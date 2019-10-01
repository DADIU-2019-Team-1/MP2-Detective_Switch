using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor.Animations;

public class TestAnims : MonoBehaviour
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
    private List<Trajectory> animTrajectories;
    private Trajectory movementTrajectory;
    private Animator animator;

    // --- Public variables
    public AnimationClip currentClip;
    public int currentFrame;
    public int currentAnimId;
    public float queryRate;
    public float comparisonThreshold = 50;

    // --- Private variables
    [SerializeField] bool isMMRunning;
    [SerializeField] int framesToCull = 10;
    private AnimationClip[] allClips;
    private float normalizedFrameTime;
    private int candidateId;
    private List<Trajectory> candidates;


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
        PlayAnimationAtFrame(currentClip.name, currentFrame / currentClip.frameRate, currentAnimId);
    }

    void PlayAnimationAtFrame(string animName, float normalizedTime, int animId)
    {
        animator.CrossFadeInFixedTime(animName, 0.3f, 0, normalizedTime); // 0.3f was recommended by Magnus
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
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < animTrajectories[currentAnimId].GetTrajectoryPoints().Length; i++)
            {
                Gizmos.DrawWireSphere(animTrajectories[currentAnimId].GetTrajectoryPoints()[i].position, movement.gizmoSphereSize);
                Gizmos.DrawLine(animTrajectories[currentAnimId].GetTrajectoryPoints()[i].position, animTrajectories[currentAnimId].GetTrajectoryPoints()[i].position + animTrajectories[currentAnimId].GetTrajectoryPoints()[i].forward);
            }
        }
    }

    private void CandidatesToCull()
    {

    }

    private IEnumerator StartMM()
    {
        // TODO: Add a banlist - discard x previous frames
        isMMRunning = true;
        while (true)
        {
            PlayAnimationAtFrame(preprocess.poses[candidateId].GetClipName(), (float)preprocess.poses[candidateId].GetFrame() / 1 / 30, candidateId);
            yield return new WaitForSeconds(queryRate);
        }
    }

    private IEnumerator StopMM()
    {
        yield return new WaitForSeconds(queryRate);
    }
    private IEnumerator StartIdle()
    {
        Debug.Log("Idling");
        PlayAnimationAtFrame(preprocess.poses[currentAnimId].GetClipName(), (float)preprocess.poses[currentAnimId].GetFrame() / 1 / 30, 0);
        yield return new WaitForSeconds(1);
    }
    private IEnumerator StartMovement()
    {
        yield return new WaitForSeconds(queryRate);
    }
}