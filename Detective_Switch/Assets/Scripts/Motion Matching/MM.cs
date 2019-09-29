using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor.Animations;

public class MM : MonoBehaviour
{
    /* Main script for handling Motion Matching.
     * Load the feature list from the preprocessor,
     * and the desired trajectory from the TrajectoryCalculator,
     * and applies motion matching to the character.
     */

    // TODO: First find the best matches for curve matching, then find the best pose in these curve animations

    // TODO: Implement a culling of recently played animation frames.

    // TODO: Take the player trajectory from player class (DONE), create a method for finding candidates (5 best?!) and comparing it to the player
    // TODO: trajectory, then whichever candidates pass the trajectory match, will be compared with posematching, where best becomes new (based on ID?)

    // --- References
    private MMPreProcessing preprocess;
    private TrajectoryTest movement;
    private Trajectory trajectory;
    private List<Trajectory> animTrajectories;
    private Trajectory movementTrajectory;
    private MMPose pose;
    private Animator animator;

    // --- Public variables
    public AnimationClip currentClip;
    public int currentFrame;
    public int currentAnimId;

    // --- Private variables
    private AnimationClip[] allClips;
    private float normalizedFrameTime;

    void Start()
    {
        // Data is loaded from the MMPreProcessor
        preprocess = GetComponent<MMPreProcessing>();
        movement = GetComponent<TrajectoryTest>();


        // Animator is initialized
        animator = GetComponent<Animator>();
        allClips = animator.runtimeAnimatorController.animationClips;

        // Populate trajectory
        animTrajectories = preprocess.GetTrajectories();
        movementTrajectory = new Trajectory(movement.GetMovementTrajectoryPoints());

        // Play the default animation and update the reference
        currentClip = allClips[0];
        currentFrame = 0;
        currentAnimId = 0;
        PlayAnimationAtFrame(currentClip.name, currentFrame / currentClip.frameRate, 0);
    }

    // LateUpdate is called once per frame, after Update()
    void LateUpdate()
    {
        movementTrajectory = new Trajectory(movement.GetMovementTrajectoryPoints());
        TrajectoryMatching(); // TODO: Limit this to be called every x'th frame
    }
    

    void TrajectoryMatching()
    {
        // TODO Step 1: compare positions and forwards, and store them in a list if they are below a threshold
        // Debug log for trajectory comparison
        foreach (Trajectory candidate in animTrajectories)
        {
            //Debug.Log("Position comparison of candidate (ID: " + candidate.GetTrajectoryId() + ") yields: " + candidate.CompareTrajectoryPoints(movementTrajectory));
            //Debug.Log("Forward comparison of candidate (ID: " + candidate.GetTrajectoryId() + ") yields: " + candidate.CompareTrajectoryForwards(movementTrajectory));
        }

        List<Trajectory> trajectoryCandidates = new List<Trajectory>();
        const float threshold = 50f;

        foreach (Trajectory candidate in animTrajectories)
        {
            if (candidate.CompareTrajectoryPoints(movementTrajectory) +
                candidate.CompareTrajectoryForwards(movementTrajectory) < threshold)
            {
                trajectoryCandidates.Add(candidate);
            }
        }
        PoseMatching(trajectoryCandidates);
    }

    void PoseMatching(List<Trajectory> candidates)
    {
        float poseDistance = float.MaxValue;
        int newId = 0;
        foreach (var candidate in candidates)
        {
            // TODO: Baller
            float candidateDistance =  0/*preprocess.poses[candidate.GetTrajectoryId()].ComparePose(new MMPose(transform.position, lFootPos, rFootPos,
                movement.GetVelocity(root), movement.GetVelocity(lFoot), movement.GetVelocity(rFoot))); */;
            if (candidateDistance < poseDistance)
            {
                poseDistance = candidateDistance;
                newId = candidate.GetTrajectoryId();
            }
        }
        // need better way of converting frame to time
        PlayAnimationAtFrame(preprocess.poses[newId].GetClipName(),(float)preprocess.poses[newId].GetFrame()/1/30, newId);
    }

    void PlayAnimationAtFrame(string animName, float normalizedTime, int animId)
    {
        animator.Play(animName, 0, normalizedTime);
        UpdateCurrentClip(animName, normalizedTime, animId);
    }

    void UpdateCurrentClip(string nameOfNewClip, float time, int clipId)
    {
        for (int i = 0; i < allClips.Length; i++)
        {
            if (allClips[i].name == nameOfNewClip)
            {
                Debug.Log("Current clip has changed from " + currentClip.name + " to " + allClips[i].name); // Keep
                currentClip = allClips[i];
                currentFrame = (int)(time * currentClip.frameRate);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {

    }
}