using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMPreProcessing
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
    /// We are supposed to use fbx data, but since i don't know how to do that, ill put it in as rig
    private AnimationClip clip;
    private int animTime; 
    private List<Vector3> jointPositions;


    public MMPreProcessing(AnimationClip _clip, int _animTime, List<Vector3> _jointPositions)
    {
        /// Pose
        clip = _clip;
        animTime = _animTime;
        jointPositions = _jointPositions;

        /// Trajectory
    }

    /// Pose references
    public AnimationClip GetAnimClip()
    {
        return clip;
    }

    public int GetAnimTime()
    {
        return animTime;
    }

    public List<Vector3> GetJointPositions()
    {
        return jointPositions;
    }
    
    /// Trajectory references
    
}
