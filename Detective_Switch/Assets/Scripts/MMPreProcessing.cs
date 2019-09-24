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

    public List<Vector3> rootPos;
    public List<Quaternion> rootQ;
    public List<Vector3> lFootPos;
    public List<Vector3> rFootPos;

    public List<Vector3> rootVel;
    public List<Vector3> lFootVel;
    public List<Vector3> rFootVel;
    // Note: We are having trouble tracking the position of the neck, since it is in muscle space.

    private void Awake()
    {
        // Populate joint- and quaternion lists
        foreach (AnimationClip clip in clips)
        {
            rootPos.AddRange(GetJointPositionsFromClipBindings(clip, jointNames[0]));
            rootQ.AddRange(GetJointQuaternionsFromClipBindings(clip, jointNames[1]));
            lFootPos.AddRange(GetJointPositionsFromClipBindings(clip, jointNames[2]));
            rFootPos.AddRange(GetJointPositionsFromClipBindings(clip, jointNames[3]));
        }
        for (int i = 0; i < rootPos.Count; i++)
        {
            if (i > 0)
            {
                rootVel.Add(CalculateVelocityFromVectors(rootPos[i], rootPos[i - 1]));
                lFootVel.Add(CalculateVelocityFromVectors(lFootPos[i], lFootVel[i - 1]));
                rFootVel.Add(CalculateVelocityFromVectors(rFootPos[i], rFootVel[i - 1]));
            }
            else
            {
                rootVel.Add(CalculateVelocityFromVectors(rootPos[i], new Vector3(0, 0, 0)));
                lFootVel.Add(CalculateVelocityFromVectors(lFootPos[i], new Vector3(0, 0, 0)));
                rFootVel.Add(CalculateVelocityFromVectors(rFootPos[i], new Vector3(0, 0, 0)));
            }
        }

        CSVReaderWriter CSVdata = new CSVReaderWriter();
        CSVdata.CSVWriteTester(rootPos, rootQ, lFootPos, rFootPos, lFootVel, rFootVel, rootVel);
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

    public Vector3 CalculateVelocityFromVectors(Vector3 currentPos, Vector3 prevPos)
    {
        return (currentPos - prevPos) / (1 / 30);
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