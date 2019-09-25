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
    private Animator animator;
    private AnimationUtility animUtil;

    // --- Inspector
    public List<AnimationClip> clips;
    public List<string> jointNames;

    private List<MMPose> poses;
    private List<TrajectoryPoint> trajectoryPoints;
    public List<Vector3> rootPos, lFootPos, rFootPos;
    public List<Quaternion> rootQ;

    // --- Not in Inspector
    [HideInInspector]
    public List<string> clipNames;
    [HideInInspector]
    public List<int> frames;
    // Note: We are having trouble tracking the position of the neck, since it is in muscle space.

    private void Awake()
    {
        poses = new List<MMPose>(); 
        trajectoryPoints = new List<TrajectoryPoint>();
        int uniqueIDIterator = 0;

        foreach (AnimationClip clip in clips)
        {
            for (int i = 0; i < clip.length * clip.frameRate; i++)
            {
                // Adding root data to list
                rootPos.Add(GetJointPositionAtFrame(clip, i, jointNames[0]));
                rootQ.Add(GetJointQuaternionAtFrame(clip, i, jointNames[1]));
                Debug.Log(rootQ[i]);
                // Creating a root transform matrix, then multiplying its inverse to transform joints to character space
                Matrix4x4 rootTrans = Matrix4x4.identity;
                Debug.Log("Position is: " + rootPos[i] + " - Quaternion is: " + rootQ[i]);
                Quaternion quart = rootQ[i];
                quart.eulerAngles = new Vector3(rootQ[i].eulerAngles.x, rootQ[i].eulerAngles.y, rootQ[i].eulerAngles.z);
                rootTrans.SetTRS(rootPos[i], quart, new Vector3(1, 1, 1));
                Debug.Log(rootTrans);
                rootQ[i] = quart;
                Debug.Log("Just quaternion at position " + i + ": " + rootQ[i]);
                lFootPos.Add(rootTrans.inverse.MultiplyPoint3x4(GetJointPositionAtFrame(clip, i, jointNames[2])));
                rFootPos.Add(rootTrans.inverse.MultiplyPoint3x4(GetJointPositionAtFrame(clip, i, jointNames[3])));

                // Add pose data to list
                if (i > 0)
                {
                    poses.Add(new MMPose(clip.name, i,
                        rootPos[i], lFootPos[i], rFootPos[i],
                        CalculateVelocityFromVectors(rootPos[i], rootPos[i - 1]),
                        CalculateVelocityFromVectors(lFootPos[i], lFootPos[i - 1]),
                        CalculateVelocityFromVectors(rFootPos[i], rFootPos[i - 1]),
                        rootQ[i]));
                }
                else // There is no previous position for velocity calculation at frame 0
                {
                    poses.Add(new MMPose(clip.name, i,
                        rootPos[i], lFootPos[i], rFootPos[i],
                        CalculateVelocityFromVectors(rootPos[i], new Vector3(0, 0, 0)),
                        CalculateVelocityFromVectors(lFootPos[i], new Vector3(0, 0, 0)),
                        CalculateVelocityFromVectors(rFootPos[i], new Vector3(0, 0, 0)),
                        rootQ[i]));
                }
                // Add trajectory to list
                trajectoryPoints.Add(new TrajectoryPoint(rootPos[i], rootQ[i] * Vector3.forward));

                uniqueIDIterator++;
            }
        }
        CSVReaderWriter CSVdata = new CSVReaderWriter();
        CSVdata.WriteCSV(poses, trajectoryPoints);
        CSVdata.ReadCSV();
    }

    public Vector3 GetJointPositionAtFrame(AnimationClip clip, int frame, string jointName)
    {
        /// Bindings are inherited from a clip, and the AnimationCurve is inherited from the clip's binding
        AnimationCurve curve = new AnimationCurve();
        float[] vectorValues = new float[3];
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
        return new Vector3(vectorValues[0], vectorValues[1], vectorValues[2]);
    }

    public Quaternion GetJointQuaternionAtFrame(AnimationClip clip, int frame, string jointName)
    {
        /// Bindings are inherited from a clip, and the AnimationCurve is inherited from the clip's binding
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

    public List<Vector3> GetJointPositionsFromClipBindings(AnimationClip clip, string jointName)
    {
        /// Bindings are inherited from a clip, and the AnimationCurve is inherited from the clip's binding
        
        List<Vector3> jointsToReturn = new List<Vector3>();

        AnimationCurve curve;
        List<float> vectorValues;
        int arrayEnumerator = 0;
        for (int i = 0; i < clip.length * clip.frameRate; i++)
        {
            vectorValues = new List<float>();
            arrayEnumerator = 0;
            foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(clip))
            {
                if (binding.propertyName.Contains(jointName))
                {
                    curve = AnimationUtility.GetEditorCurve(clip, binding);
                    vectorValues.Add(curve.Evaluate(i / clip.frameRate));
                    arrayEnumerator++;
                }
            }
            jointsToReturn.Add(new Vector3(vectorValues[0], vectorValues[1], vectorValues[2]));
        }

        return jointsToReturn;
    }

    public List<Quaternion> GetJointQuaternionsFromClipBindings(AnimationClip clip, string jointName)
    {
        /// Bindings are inherited from a clip, and the AnimationCurve is inherited from the clip's binding
        List<Quaternion> quaternionsToReturn = new List<Quaternion>();

        AnimationCurve curve;
        List<float> vectorValues;
        int arrayEnumerator = 0;
        for (int i = 0; i < clip.length * clip.frameRate; i++)
        {
            vectorValues = new List<float>();
            arrayEnumerator = 0;
            foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(clip))
            {
                if (binding.propertyName.Contains(jointName))
                {
                    curve = AnimationUtility.GetEditorCurve(clip, binding);
                    vectorValues.Add(curve.Evaluate(i / clip.frameRate));
                    arrayEnumerator++;
                }
            }
            quaternionsToReturn.Add(new Quaternion(vectorValues[0], vectorValues[1], vectorValues[2], vectorValues[3]));
        }

        return quaternionsToReturn;
    }

    private List<string> GetClipNameFromClip(AnimationClip clip)
    {
        List<string> _clipNames = new List<string>();

        for (int i = 0; i < clip.length * clip.frameRate; i++)
        {
            _clipNames.Add(clip.name);
        }

        return _clipNames;
    }

    private List<int> GetFramesFromClip(AnimationClip clip)
    {
        List<int> _frames = new List<int>();
        int currentFrame = 0;
        int indexer = 0;

        for (int i = 0; i < clip.length * clip.frameRate; i++)
        {
            indexer++;
            if (indexer == clip.frameRate)
            {
                currentFrame++;
                indexer = 0;
            }

        _frames.Add(currentFrame);
        }

        return _frames;
    }

    public Vector3 CalculateVelocityFromVectors(Vector3 currentPos, Vector3 prevPos)
    {
        return (currentPos - prevPos) / 1 / 30;
    }

    // Trajectory references
    
}

//public class FeaturePoseVector
//{
//    private List<Vector3> leftFoot;
//    private List<Vector3> rightFoot;
//    private List<Vector3> spine;
//    private List<float> footVelocity;

//    public FeaturePoseVector(List<Vector3> _leftFoot, List<Vector3> _rightFoot, List<Vector3> _spine, List<float> _footVelocity)
//    {
//        leftFoot = _leftFoot;
//        rightFoot = _rightFoot;
//        spine = _spine;
//        footVelocity = _footVelocity;
//    }

//    public List<Vector3> GetLeftFoot()
//    {
//        return leftFoot;
//    }

//    public List<Vector3> GetRightFoot()
//    {
//        return rightFoot;
//    }

//    public List<Vector3> GetSpine()
//    {
//        return spine;
//    }

//    public List<float> GetFootVelocity()
//    {
//        return footVelocity;
//    }

//    public FeaturePoseVector GetFeaturePoseVector()
//    {
//        return this;    // Don't know if you can do this?
//    }
//}

//// Attempting to access curves through clips
//List<AnimationCurve> curves = new List<AnimationCurve>();
//int num = 0;
//foreach (AnimationClip clip in clips)
//{
//    foreach (var binding in AnimationUtility.GetCurveBindings(clip))
//    {
//        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
//        for (int i = 0; i < clip.length * clip.frameRate; i++)
//            if (binding.propertyName.Contains("Root"))
//                Debug.Log("Curve for " + binding.propertyName + " evaluation at frame " + i + ": " + curve.Evaluate(i / clip.frameRate));
//    }
//    Debug.Log("FINISHED CLIP " + num + "!");
//    num++;
//}