using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MMPreProcessing : MonoBehaviour
{
    /* The purpose of this script is to process and store the
     * pose and trajectory in a feature list, which must cached
     * such that it can simply be read when the game starts.
     * However, initially we will simply test this by preprocessing
     * on start (or awake).
     *
     * Pose contains relevant joints (feet, neck), relevant velocities,
     * and other information regarding the joints in the rig.
     * Trajectory contains relevant information about the current trajectory
     * when playing a specific animation. It contains the time steps that is
     * being processed, and the amount of time to look in advance.
     */

    // --- References

    // --- Public 
    [HideInInspector] public List<MMPose> poses;
    [HideInInspector] public List<TrajectoryPoint> trajectoryPoints;
    [HideInInspector] public List<Trajectory> trajectories;
    [HideInInspector] public List<string> clipNames;
    [HideInInspector] public List<int> clipFrames;
    public List<AnimationClip> clips;
    public List<string> jointNames;
    public bool preprocess = false;
    public int trajectoryPointsToUse = 4;
    public int frameStepSize = 25;

    // --- Private
    private List<Vector3> rootPos, lFootPos, rFootPos;
    private List<Quaternion> rootQ, lFootQ, rFootQ;
    // Note: We are having trouble tracking the position of the neck, since it is in muscle space.

    private void Awake()
    {
	    InitCollections();
        clips.AddRange(GetComponent<Animator>().runtimeAnimatorController.animationClips);
        for (int i = 0; i < clips.Count; i++)
        {
	        if (clips[i].name.Contains("Loop")) // We cull loop animations from the csv
				clips.RemoveAt(i);
        }
		CSVReaderWriter csvHandler = new CSVReaderWriter();
        if (preprocess)
        {
            foreach (AnimationClip clip in clips)
            {
                // Initialize lists again to avoid reusing previous data
                clipNames = new List<string>();
                clipFrames = new List<int>();
                rootPos = new List<Vector3>();
                lFootPos = new List<Vector3>();
                rFootPos = new List<Vector3>();
                rootQ = new List<Quaternion>();
                lFootQ = new List<Quaternion>();
                rFootQ = new List<Quaternion>();
                for (int i = 0; i < clip.length * clip.frameRate; i++)
                {
                    clipNames.Add(clip.name);
                    clipFrames.Add(i);
                    // Adding root data to list
                    rootPos.Add(GetJointPositionAtFrame(clip, i, jointNames[0] + "T"));
                    rootQ.Add(GetJointQuaternionAtFrame(clip, i, jointNames[0] + "Q"));
                    /// The approach below sounds good in theory, but in this implementation did not yield expected results.
                    /// Upon further inspection, simply removing the root would suffice for the desired result.
                    //// Creating a root transform matrix, then multiplying its inverse to transform joints to character space
                    //Matrix4x4 rootTrans = Matrix4x4.identity;
                    //Quaternion quart = rootQ[i];
                    //quart.eulerAngles = new Vector3(rootQ[i].eulerAngles.x, rootQ[i].eulerAngles.y, rootQ[i].eulerAngles.z);
                    //rootTrans.SetTRS(rootPos[i], quart, new Vector3(1, 1, 1));
                    //rootQ[i] = quart;
                    //lFootPos.Add(rootTrans.inverse.MultiplyPoint3x4(GetJointPositionAtFrame(clip, i, jointNames[2])));
                    //rFootPos.Add(rootTrans.inverse.MultiplyPoint3x4(GetJointPositionAtFrame(clip, i, jointNames[3])));
                    lFootPos.Add(GetJointPositionAtFrame(clip, i, jointNames[1] + "T") - rootPos[i]);
                    lFootQ.Add(GetJointQuaternionAtFrame(clip, i, jointNames[1] + "Q"));
                    rFootPos.Add(GetJointPositionAtFrame(clip, i, jointNames[2] + "T") - rootPos[i]);
                    rFootQ.Add(GetJointQuaternionAtFrame(clip, i, jointNames[2] + "Q"));


                    // Add pose data to list
                    if (i >= frameStepSize)
                    {
                        poses.Add(new MMPose(clip.name, i,
                            rootPos[i], lFootPos[i], rFootPos[i],
                            CalculateVelocityFromVectors(rootPos[i], rootPos[i - frameStepSize]),
                            CalculateVelocityFromVectors(lFootPos[i], lFootPos[i - frameStepSize]),
                            CalculateVelocityFromVectors(rFootPos[i], rFootPos[i - frameStepSize]),
                            rootQ[i],lFootQ[i],rFootQ[i]));
                    }
                    else // There is no previous position for velocity calculation at frame 0
                    {
                        poses.Add(new MMPose(clip.name, i,
                            rootPos[i], lFootPos[i], rFootPos[i],
                            CalculateVelocityFromVectors(rootPos[i], Vector3.zero),
                            CalculateVelocityFromVectors(lFootPos[i], Vector3.zero),
                            CalculateVelocityFromVectors(rFootPos[i], Vector3.zero),
                            rootQ[i], lFootQ[i], rFootQ[i]));
                    }
                    trajectoryPoints.Add(new TrajectoryPoint(rootPos[i] - rootPos[0], rootQ[i] * Vector3.forward)); // subtract 0 pos, which sets the starting point to 0.
                }
            }
            csvHandler.WriteCSV(poses, trajectoryPoints);
        }

        InitCollections();
        csvHandler.ReadCSV();
        for (int i = 0; i < csvHandler.GetClipNames().Count; i++)
        {
            clipNames.Add(csvHandler.GetClipNames()[i]);
            clipFrames.Add(csvHandler.GetFrames()[i]);

            // Read pose data and store it in a list of poses
            poses.Add(new MMPose(clipNames[i], clipFrames[i],
                csvHandler.GetRootPos()[i], csvHandler.GetLeftFootPos()[i], csvHandler.GetRightFootPos()[i],
                csvHandler.GetLeftFootVel()[i], csvHandler.GetRightFootVel()[i], csvHandler.GetLeftFootQs()[i], csvHandler.GetRightFootQs()[i]));

            // To add the trajectory data, first compute the trajectory points for each frame
            TrajectoryPoint[] tempPoints = new TrajectoryPoint[trajectoryPointsToUse];
            if (i + (frameStepSize * trajectoryPointsToUse) < csvHandler.GetClipNames().Count && // Avoid out-of-bounds error
                csvHandler.GetClipNames()[i] == csvHandler.GetClipNames()[i + (frameStepSize * trajectoryPointsToUse)]) // Make sure frames belong to the same clip
            {
                for (int point = 0; point < tempPoints.Length; point++)
                {
                    tempPoints[point] = new TrajectoryPoint(csvHandler.GetTrajectoryPos()[i + (point * frameStepSize)],
                        csvHandler.GetTrajectoryForwards()[i + (point * frameStepSize)]);
                }
                trajectories.Add(new Trajectory(csvHandler.GetClipNames()[i], csvHandler.GetFrames()[i], i, tempPoints));
            }
            else
            {
                for (int j = 0; j < trajectoryPointsToUse; j++)
                    tempPoints[j] = new TrajectoryPoint();
                trajectories.Add(new Trajectory(tempPoints));
            }
        }
    }

    private void InitCollections()
    {
        poses = new List<MMPose>();
        trajectoryPoints = new List<TrajectoryPoint>();
        clipNames = new List<string>();
        clipFrames = new List<int>();
        rootPos = new List<Vector3>();
        lFootPos = new List<Vector3>();
        rFootPos = new List<Vector3>();
        rootQ = new List<Quaternion>();
        lFootQ = new List<Quaternion>();
        rFootQ = new List<Quaternion>();
    }

    public List<Trajectory> GetTrajectories()
    {
        return trajectories;
    }

    public Vector3 GetJointPositionAtFrame(AnimationClip clip, int frame, string jointName)
    {
        // Bindings are inherited from a clip, and the AnimationCurve is inherited from the clip's binding
        float[] vectorValues = new float[3];
        int arrayEnumerator = 0;
        foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(clip))
        {
            if (binding.propertyName.Contains(jointName))
            {
                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                vectorValues[arrayEnumerator] = curve.Evaluate(frame / clip.frameRate);
                arrayEnumerator++;
            }
        }
        return new Vector3(vectorValues[0], vectorValues[1], vectorValues[2]);
    }

    public Quaternion GetJointQuaternionAtFrame(AnimationClip clip, int frame, string jointName)
    {
        // Bindings are inherited from a clip, and the AnimationCurve is inherited from the clip's binding
        AnimationCurve curve = new AnimationCurve();
        float[] vectorValues = new float[4];
        int arrayEnumerator = 0;
        foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(clip))
        {
            if (binding.propertyName.Contains(jointName))
            {
                curve = AnimationUtility.GetEditorCurve(clip, binding);
                vectorValues[arrayEnumerator] = curve.Evaluate(frame / clip.frameRate);
                arrayEnumerator++;
            }
        }
        return new Quaternion(vectorValues[0], vectorValues[1], vectorValues[2], vectorValues[3]);
    }

    public Vector3 CalculateVelocityFromVectors(Vector3 currentPos, Vector3 prevPos)
    {
        return (currentPos - prevPos) / 1 / 30;
    }
}