using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.UIElements;

public class MM : MonoBehaviour
{
    /* Main script for handling Motion Matching.
     * Load the feature list from the preprocessor,
     * and the desired trajectory from the TrajectoryCalculator,
     * and applies motion matching to the character.
     */

    // --- References
    private MMPreProcessing preprocess;
    private TrajectoryTest movement;
    private List<Trajectory> animTrajectories;
    Trajectory[] animTrajectoriesInCharSpace;
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
    [SerializeField] private int candidateId;
    private List<Trajectory> candidates;
    private Queue<int> culledIDs;

    void Start()
    {
        // --- Loading references
        preprocess = GetComponent<MMPreProcessing>();
        movement = GetComponent<TrajectoryTest>();
        animator = GetComponent<Animator>();

        // --- Initializing collections
        allClips = animator.runtimeAnimatorController.animationClips;
        animTrajectories = preprocess.GetTrajectories();
		animTrajectoriesInCharSpace = new Trajectory[animTrajectories.Count];
        movementTrajectory = new Trajectory(movement.GetMovementTrajectoryPoints());
        culledIDs = new Queue<int>();
        for (int i = 0; i < allClips[0].length * allClips[0].frameRate; i++)
        {
	        culledIDs.Enqueue(i);
        }

        // Play the default animation and update the reference
        currentClip = allClips[0];
        currentFrame = 0;
        currentAnimId = 0;
        PlayAnimationAtFrame(currentClip.name, currentFrame / currentClip.frameRate, 0);

		// Start the idle coroutine
        StartCoroutine(StartMM());
    }

    private void Update()
    {
	    if (movement.rootVel.sqrMagnitude >= 0.0001f && !isMMRunning)
	    {
		    Debug.Log("Starting MM Coroutine!");
            StopAllCoroutines();
		    StartCoroutine(StartMM());
	    }
	    if (movement.rootVel.sqrMagnitude < 0.0001f && isMMRunning)
        { 
			Debug.Log("Starting Idle Coroutine!");
		    StopAllCoroutines();
		    isMMRunning = false;
		    StartCoroutine(StartIdle());
	    }
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
                tempPoints[j].position = transform.worldToLocalMatrix.MultiplyPoint3x4(animTrajectories[i].GetTrajectoryPoints()[j].position);
                tempPoints[j].forward = transform.worldToLocalMatrix.MultiplyVector(animTrajectories[i].GetTrajectoryPoints()[j].forward);
            }
            animTrajectoriesInCharSpace[i] = new Trajectory(animTrajectories[i].GetClipName(), animTrajectories[i].GetFrame(), animTrajectories[i].GetTrajectoryId(), tempPoints);
        }
		List<Trajectory> trajectoryCandidates = new List<Trajectory>();
		for (int i = 0; i < animTrajectoriesInCharSpace.Length; i++)
        {
            if (animTrajectoriesInCharSpace[i].GetTrajectoryId() >= 0 && animTrajectoriesInCharSpace[i].GetTrajectoryId() != currentAnimId
                && !culledIDs.Contains(animTrajectoriesInCharSpace[i].GetTrajectoryId()))
            {
                if (animTrajectoriesInCharSpace[i].CompareTrajectoryPoints(movement) +
                    animTrajectoriesInCharSpace[i].CompareTrajectoryForwards(movement) < threshold)
                {
                    trajectoryCandidates.Add(animTrajectoriesInCharSpace[i]);
                }
            }
        }
        return trajectoryCandidates;
    }

    private int PoseMatching(List<Trajectory> candidates)
    {
        float poseDistance = float.MaxValue;
        int newId = currentAnimId;
        foreach (var candidate in candidates)
        {
            float candidateDistance = preprocess.poses[candidate.GetTrajectoryId()].ComparePose(new MMPose(
                movement.root.position, movement.lFoot.position, movement.rFoot.position,
                movement.rootVel, movement.lFootVel, movement.rFootVel));
            if (candidateDistance < poseDistance)
            {
                poseDistance = candidateDistance;
                newId = candidate.GetTrajectoryId();
            }
        }
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
            if (allClips[i].name == nameOfNewClip)
            {
                Debug.Log("Current clip has changed from " + currentClip.name + " to " + allClips[i].name); // Keep
                currentClip = allClips[i];
                currentFrame = (int)(time * currentClip.frameRate);
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
                Gizmos.DrawLine(animTrajectoriesInCharSpace[currentAnimId].GetTrajectoryPoints()[i].position, animTrajectoriesInCharSpace[currentAnimId].GetTrajectoryPoints()[i].position + animTrajectoriesInCharSpace[currentAnimId].GetTrajectoryPoints()[i].forward);
            }
        }
    }

    private void UpdateQueue(int idToCull) 
    {
        culledIDs.Enqueue(idToCull);
        if (culledIDs.Count >= framesToCull + allClips[0].length * allClips[0].frameRate)
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
	    while (true)
	    {
		    PlayAnimationAtFrame(preprocess.poses[0].GetClipName(), (float)preprocess.poses[0].GetFrame() / 1 / 30, 0);
		    yield return new WaitForSeconds(currentClip.length);
        }
    }

	// StartMovement() is not implemented in this project, but the idea is that if the current trajectory is = to the desired trajectory,
	// we stop MotionMatching and simply play the movement movement animation corresponding to the trajectories.
	//private IEnumerator StartMovement()
 //   {
 //       yield return new WaitForSeconds(queryRate);
 //   }
}