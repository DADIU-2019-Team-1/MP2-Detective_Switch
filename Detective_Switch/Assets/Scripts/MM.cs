using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor.Animations;

public class MM : MonoBehaviour
{
    /* Main script for handling Motion Matching.
     * Load the feature list from the preprocesser,
     * and the desired trajectory from the TrajectoryCalculator,
     * and applies motion matching to the character.
     */
    private MMPreProcessing preprocces;
    private FeaturePoseVector featurePoseVector;
    private TrajectoryTest trajectory;
    private TrajectoryCalculator trajectoryCalc;
    private Animator animator;
    public AnimationClip currentClip;
    public int currentAnimTime;
    public List<Vector3> jointPosList;


    void Start()
    {
        /// This is where the data would be loaded from a file, but for debugging
        /// purposes we simply run the preprocesser script with an input.
        /// Preprocesser does not derive from monobehaviour, and as such does not need
        /// an object reference.
        animator = GetComponent<Animator>();
        AnimatorController controller = GetComponent<AnimatorController>();

        AnimatorStateInfo currentAnimatorStateInfo;
        //float playbackTime = currentAnimatorStateInfo.normalizedTime * currentAnimatorStateInfo.length;

        AnimationClip[] manyClips = controller.animationClips;
        List<AnimationClip> allClips = new List<AnimationClip>();

        // NEED A SMARTER WAY TO ITERATE THROUGHT THE AMOUNT OF CLIPS (STATES) IN AN ANIMATION
        for (int i = 0; i < manyClips.Length; i++)
        {
            Debug.Log(manyClips[i]);
            //allClips = controller.animationClips;
            //currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(i);
            //Debug.Log("Name of animation " + i + ": " + currentAnimatorStateInfo.nameHash);
            //Debug.Log("Length of animation " + i +  ": " + currentAnimatorStateInfo.length);
        }

        //preprocces = new MMPreProcessing(animator.GetCurrentAnimatorClipInfo)
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }

    void ComputeCost()
    {

    }

    void NewClipSelector()
    {

    }

}
