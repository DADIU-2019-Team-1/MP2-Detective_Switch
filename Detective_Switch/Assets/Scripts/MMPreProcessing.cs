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
    private Animator animator;
    private AnimationUtility animUtil;
    public List<AnimationClip> clips;
    private float animTime; 
    private List<Vector3> jointPositions;
    // Curve bindings, animationsutility, evaluate

    private void Start()
    {
        // Attempting to access curves through clips
        List<AnimationCurve> curves = new List<AnimationCurve>();
        int num = 1;
        foreach (AnimationClip clip in clips)
        {
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                for (int i = 0; i < clip.length * clip.frameRate; i++)
                    Debug.Log("Curve for " + binding.propertyName + " evaluation at frame " + i + ": " + curve.Evaluate(i / clip.frameRate));
            }
            Debug.Log("FINISHED CLIP " + num + "!");
        }
    }

    public MMPreProcessing(List<AnimationClip> _clips, float _animTime, List<Vector3> _jointPositions)
    {
        // Pose
        clips = _clips;
        animTime = _animTime;
        jointPositions = _jointPositions;

        // Trajectory
    }

    /// Pose references
    public List<AnimationClip> GetAllClips()
    {
        return clips;
    }

    public AnimationClip GetClip(int clipNum)
    {
        return clips[clipNum];
    }

    public float GetAnimTime()
    {
        return animTime;
    }

    public List<Vector3> GetJointPositions()
    {
        return jointPositions;
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
