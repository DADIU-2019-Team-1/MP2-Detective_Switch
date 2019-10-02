using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    // --- References
    Rigidbody playerRB;
    MMPreProcessing preProcessing;




    // --- Public
    public float maxPlayerSpeed = 8.5f, minDragToMove = 70, maxDragToMove = 250, maxPressTime = 0.15f, minPlayerSpeed = 5, 
	    moveReactionTime = 0.3f, turnReactionTime = 2.5f, globalPlayerSpeed, gizmoSphereSize = 0.2f;
	[HideInInspector] public TrajectoryPoint[] trajectoryPoints;

	public Transform root, lFoot, rFoot;
	[HideInInspector] public Vector3 rootVel, lFootVel, rFootVel;



    // --- Private
    private Vector2 joyAnchor;
    private Vector3 oldPos;
    private Vector3 prevLocation;
    private Vector3 rootPrePos, lFootPrePos, rFootPrePos,
	    moveVector, direction;
    private bool canMove, mouseDown;
    public float currentPlayerSpeed;
    public float interactDistance = 4.0f;
    private float joyDisplacementAngle = -0.25f * Mathf.PI; // This converts radians, turning by 45 degrees for isometric view.
    private float playerSpeedInterval, timeAtTouchDown, distanceTravelled;


    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        preProcessing = GetComponent<MMPreProcessing>();
        trajectoryPoints = new TrajectoryPoint[preProcessing.trajectoryPointsToUse];
        InitArray(trajectoryPoints);
        playerSpeedInterval = (maxPlayerSpeed / maxDragToMove) * 100;
        oldPos = transform.position;
        prevLocation = transform.position;
        playerRB.velocity = Vector3.zero;
    }

    private void Update()
    {
	    trajectoryPoints = CalculateTrajectory((float)preProcessing.frameStepSize / 100);
    }

    void FixedUpdate()
    {
        if (!GameMaster.instance.GetMenuIsOpen() && !GameMaster.instance.GetJournalIsOpen())
			HandleInput();
        UpdateJointVelocities();

        float tempPlayerSpeed = (transform.position - prevLocation).magnitude / Time.deltaTime;
        if (tempPlayerSpeed < 0.15f)
        {
            currentPlayerSpeed = 0;
        }
        else
        {
            currentPlayerSpeed = tempPlayerSpeed;
        }

        prevLocation = transform.position;
    }

    void HandleInput() {
        if(Input.GetMouseButtonDown(0)) {
            joyAnchor = Input.mousePosition;
            timeAtTouchDown = Time.time;
            mouseDown = true;
        }
        if(Input.GetMouseButton(0)) {
            Vector2 joyDragVector = GetJoyDragVector(joyAnchor, Input.mousePosition);
            if(joyDragVector.magnitude > minDragToMove || Time.time - timeAtTouchDown > maxPressTime) {
                canMove = true;
            }
            if(canMove) {
                MovePlayer(new Vector3(joyDragVector.x, 0, joyDragVector.y));
            }
        }
        if(Input.GetMouseButtonUp(0)) {
            if (mouseDown)
            {
                if (!canMove || Time.time - timeAtTouchDown < maxPressTime)
                {
                    MouseClick();
                } 
                else
                {
                    //lookAtPosition = transform.rotation;
                }
            }
            mouseDown = false;
            canMove = false;
        }
    }

    void MouseClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Interactable", "SolidBlock");

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
            GameObject clickedObject = hit.transform.gameObject;
            if (clickedObject.GetComponent<Interactable>())
            {
                if ((clickedObject.transform.position - transform.position).magnitude <= interactDistance)
                {
                    clickedObject.GetComponent<Interactable>().Interact();
                }
            }
        }
    }

    Vector2 GetJoyDragVector(Vector2 anchor, Vector2 goal) {
        Vector2 rawJoy = goal - anchor;
        Vector2 turnedJoyVector = new Vector2(0,0);
        turnedJoyVector.x = rawJoy.x * Mathf.Cos(joyDisplacementAngle) - rawJoy.y * Mathf.Sin(joyDisplacementAngle);
        turnedJoyVector.y = rawJoy.x * Mathf.Sin(joyDisplacementAngle) + rawJoy.y * Mathf.Cos(joyDisplacementAngle);
        turnedJoyVector = Vector2.ClampMagnitude(turnedJoyVector, maxDragToMove);
        return turnedJoyVector;
    }

    void MovePlayer(Vector3 moVector) 
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position + moVector), turnReactionTime * Time.deltaTime);

        Vector3 speedMove = (moVector / maxDragToMove) * maxPlayerSpeed;

        if (speedMove.magnitude < minPlayerSpeed)
        {
            speedMove = speedMove.normalized * minPlayerSpeed;
        }

        speedMove.y = playerRB.velocity.y;

        playerRB.velocity = speedMove; // Vector3.Lerp(playerRB.velocity, speedMove * 10, moveReactionTime * Time.deltaTime);

        distanceTravelled = (oldPos - transform.position).magnitude;
        oldPos = transform.position;
        GameMaster.instance.SetMoveSpeed(globalPlayerSpeed);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    TrajectoryPoint[] CalculateTrajectory(float timeStep)
    {
	    TrajectoryPoint[] point = new TrajectoryPoint[trajectoryPoints.Length];
	    InitArray(point);
	    for (int i = 0; i < point.Length; i++)
	    {
		    if (i != 0)
		    {
			    // Quaternion.LookRotation spams debug errors when input is vector3.zero, this removes that possibility
			    Quaternion lookRotation = transform.position + moveVector != Vector3.zero ? Quaternion.LookRotation(transform.position + moveVector) : Quaternion.identity;

			    point[i].position = point[i - 1].position + Quaternion.Slerp(transform.rotation, lookRotation, timeStep * i) * Vector3.forward;
			    point[i].forward = (point[i].position - point[i - 1].position).normalized;
		    }
		    else
		    {
			    point[i].position = transform.position;
			    point[i].forward = transform.forward;
		    }
	    }
	    return point;
    }
    private void UpdateJointVelocities()
    {
	    rootVel = (root.position - rootPrePos) / Time.fixedDeltaTime;
	    lFootVel = (lFoot.position - lFootPrePos) / Time.fixedDeltaTime;
	    rFootVel = (rFoot.position - rFootPrePos) / Time.fixedDeltaTime;
	    rootPrePos = root.position;
	    lFootPrePos = lFoot.position;
	    rFootPrePos = rFoot.position;
    }


    private void InitArray(TrajectoryPoint[] arr)
    {
	    for (int i = 0; i < arr.Length; i++)
	    {
		    arr[i] = new TrajectoryPoint();
	    }
    }

    public TrajectoryPoint[] GetMovementTrajectoryPoints()
    {
	    return trajectoryPoints;
    }
}
