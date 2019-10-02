using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(MMPreProcessing))]
public class MM : MonoBehaviour
{
    /* Main script for handling Motion Matching.
     * Load the feature list from the preprocessor,
     * and the desired trajectory from the TrajectoryCalculator,
     * and applies motion matching to the character.
     */

    // --- References
    private MMPreProcessing preprocess;
    private CharacterMovement movement;
    private List<Trajectory> animTrajectories;
    Trajectory[] animTrajectoriesInCharSpace;
    private Trajectory movementTrajectory;
    private Animator animator;

    // --- Public variables
    public AnimationClip currentClip;
    public AnimationClip loopWalk;
    public int currentFrame;
    public int currentAnimId;
    public float queryRate;
    public float comparisonThreshold = 50;

    // --- Private variables
    [SerializeField] bool isMMRunning, isCurTrajSameAsDesired, isIdling;
    [SerializeField] int framesToCull = 10;
    [SerializeField] private int candidateId;
    private AnimationClip[] allClips;
    private List<Trajectory> candidates;
    private Queue<int> culledIDs;

    void Start()
    {
        // --- Loading references
        preprocess = GetComponent<MMPreProcessing>();
        movement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        allClips = new AnimationClip[preprocess.clips.Count+1];
        for (int i = 0; i < allClips.Length; i++)
        {
            if (i < allClips.Length - 1)
                allClips[i] = preprocess.clips[i];
            else
                allClips[i] = loopWalk;
        }
        
        animator.applyRootMotion = false;
        movementTrajectory = new Trajectory(new TrajectoryPoint[preprocess.trajectoryPointsToUse]);

        // --- Initializing collections
        animTrajectories = preprocess.GetTrajectories();
		animTrajectoriesInCharSpace = new Trajectory[animTrajectories.Count];
        culledIDs = new Queue<int>();

        // Play the default animation and update the reference
        currentClip = allClips[0];
        currentFrame = 0;
        currentAnimId = 0;
        PlayAnimationAtFrame(currentClip.name, currentFrame / currentClip.frameRate, 0);

		// Starting MM to avoid gizmo errors, otherwise we would start Idle
        StartCoroutine(StartMM());
        //StartCoroutine(UpdateAnimationFrame());
    }

    private void Update()
    {
        if (movement.rootVel.sqrMagnitude >= 0.0001f)
        {
            float angle = 0;
            foreach (var point in movementTrajectory.GetTrajectoryPoints())
            {
                angle += Vector3.Angle(transform.forward, point.forward);
            }
            if (angle <= 0.05f && !isCurTrajSameAsDesired)
            {
                //Debug.Log("Start Movement Coroutine because angle is: " + angle + "!");
                StopAllCoroutines();
                isMMRunning = false;
                isIdling = false;
                StartCoroutine(StartMovement());
               //StartCoroutine(UpdateAnimationFrame());
            }
            else if (angle > 0.05f && !isMMRunning)
            {
                //Debug.Log("Starting MM Coroutine!");
                StopAllCoroutines();
                isIdling = false;
                isCurTrajSameAsDesired = false;
                StartCoroutine(StartMM());
                //StartCoroutine(UpdateAnimationFrame());
            }
        }
        else if (movement.rootVel.sqrMagnitude < 0.0001f && !isIdling)
        {
            //Debug.Log("Starting Idle Coroutine!");
            StopAllCoroutines();
            isCurTrajSameAsDesired = false;
            isMMRunning = false;
            StartCoroutine(StartIdle());
            //StartCoroutine(UpdateAnimationFrame());
        }
        //currentAnimId++;
        currentFrame = Mathf.RoundToInt(animator.GetCurrentAnimatorStateInfo(0).normalizedTime * currentClip.frameRate);
    }

    List<Trajectory> TrajectoryMatching(Trajectory movement, float threshold)
    {
        //Transform animation trajectories to character space

        for (int i = 0; i < animTrajectories.Count; i++)
        {
            TrajectoryPoint[] tempPoints = new TrajectoryPoint[preprocess.trajectoryPointsToUse];
            for (int j = 0; j < tempPoints.Length; j++)
                tempPoints[j] = new TrajectoryPoint(); // Initialize
            for (int j = 0; j < preprocess.trajectoryPointsToUse; j++)
            {
                tempPoints[j].position = animTrajectories[i].GetTrajectoryPoints()[j].position + transform.position;
                tempPoints[j].forward = animTrajectories[i].GetTrajectoryPoints()[j].forward + transform.position;
            }
            animTrajectoriesInCharSpace[i] = new Trajectory(animTrajectories[i].GetClipName(), animTrajectories[i].GetFrame(), animTrajectories[i].GetTrajectoryId(), tempPoints);
        }
		List<Trajectory> trajectoryCandidates = new List<Trajectory>();

        /* Culled candidates:
         * 1. Candidates with an ID below 0 (-1 is set as an invalid ID in the empty point constructor)
         * 2. Candidates pertaining to the Idle animation (ID: 0-19).
         * 3. Candidates that have been added the culledIDs queue (these have already been used)
         * 4. Candidates pertaining to the same animation as the current animation, but at a previous frame
        */
        for (int i = 0; i < animTrajectoriesInCharSpace.Length; i++)
        {
            if (animTrajectoriesInCharSpace[i].GetTrajectoryId() >= allClips[0].length * allClips[0].frameRate && 
                animTrajectoriesInCharSpace[i].GetTrajectoryId() != currentAnimId &&
                !culledIDs.Contains(animTrajectoriesInCharSpace[i].GetTrajectoryId()))
            {
                if (animTrajectoriesInCharSpace[i].GetClipName() == currentClip.name)
                {
                    if (animTrajectories[i].GetTrajectoryId() >= currentAnimId - 10 && animTrajectories[i].GetTrajectoryId() < currentAnimId)
                    {
                        continue; // Skip this candidate if it belong to the same animation, but at a previous frame
                    }
                }
                if (animTrajectoriesInCharSpace[i].CompareTrajectoryPoints(movement) +
                    animTrajectoriesInCharSpace[i].CompareTrajectoryForwards(movement) < threshold)
                {
                    //Debug.Log("TrajComparisonDist: " + animTrajectoriesInCharSpace[i].CompareTrajectoryPoints(movement) +
                    //          animTrajectoriesInCharSpace[i].CompareTrajectoryForwards(movement));
                    trajectoryCandidates.Add(animTrajectoriesInCharSpace[i]);
                }
            }
        }
        return trajectoryCandidates;
    }

    private int PoseMatching(List<Trajectory> candidates)
    {
        float poseDistance = float.MaxValue;
        int newId = 0;
        MMPose movementPose = new MMPose(transform.position, movement.lFoot.position, movement.rFoot.position, transform.rotation,
            movement.rFoot.rotation, movement.lFoot.rotation, movement.lFootVel, movement.rFootVel);

        foreach (var candidate in candidates)
        {
            MMPose candidatePose = preprocess.poses[candidate.GetTrajectoryId()].ConvertToOtherPoseSpace(movementPose);
            float candidateDistance = candidatePose.ComparePose(movementPose);
            if (candidateDistance < poseDistance)
            {
                poseDistance = candidateDistance;
                newId = candidate.GetTrajectoryId();
            }
        }
        //Debug.Log("Pose dist: " + poseDistance);
        return newId;
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
            if (allClips[i].name == nameOfNewClip && clipId >= 0)
            {
                //if (allClips[0] != currentClip)
                //    Debug.Log("Current clip has changed from " + currentClip.name + " to " + allClips[i].name + " (" + currentAnimId + "->" + clipId + ")!");
                currentClip = allClips[i];
                currentAnimId = clipId;
				if (isMMRunning)
					UpdateQueue(currentAnimId);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < animTrajectoriesInCharSpace[currentAnimId].GetTrajectoryPoints().Length; i++)
            {
                Gizmos.DrawWireSphere(animTrajectoriesInCharSpace[currentAnimId].GetTrajectoryPoints()[i].position, movement.gizmoSphereSize);
                Gizmos.DrawLine(animTrajectoriesInCharSpace[currentAnimId].GetTrajectoryPoints()[i].position, animTrajectoriesInCharSpace[currentAnimId].GetTrajectoryPoints()[i].forward);
            }
        }
    }

    private void UpdateQueue(int idToCull) 
    {
        culledIDs.Enqueue(idToCull);
        if (culledIDs.Count >= framesToCull)
            culledIDs.Dequeue();
    }

    private IEnumerator StartMM()
    {
        isMMRunning = true;
        while (true)
        {
            movementTrajectory = new Trajectory(movement.GetMovementTrajectoryPoints());
            candidates = TrajectoryMatching(movementTrajectory, comparisonThreshold);
            candidateId = PoseMatching(candidates);
            PlayAnimationAtFrame(preprocess.poses[candidateId].GetClipName(), (float)preprocess.poses[candidateId].GetFrame() / 1 / 30, candidateId);
            yield return new WaitForSeconds(queryRate);
        }
    }
    private IEnumerator StartIdle()
    {
        isIdling = true;
        while (true)
	    {
		    PlayAnimationAtFrame(preprocess.poses[0].GetClipName(), (float)preprocess.poses[0].GetFrame() / 1 / 30, 0);
		    yield return new WaitForSeconds(currentClip.length);
        }
    }
    private IEnumerator UpdateAnimationFrame()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / currentClip.frameRate);
        }
    }
    private IEnumerator StartMovement()
    {
        isCurTrajSameAsDesired = true;
        while (true)
        {
            PlayAnimationAtFrame("WalkLoop", 0, -1);
            int i = -1;
            for (int j = 0; j < allClips.Length; j++)
            {
                if (allClips[j].name == "WalkLoop")
                {
                    i = j;
                    break;
                }
            }
            yield return new WaitForSeconds(allClips[i].length);
        }
    }
}