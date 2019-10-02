using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrajectoryTest : MonoBehaviour
{
    // --- References
    Rigidbody rb;
    [HideInInspector] public TrajectoryPoint[] trajectoryPoints;
    MMPreProcessing preProcessing;

    // --- Public
    public float maxPlayerSpeed = 8.5f, minDragToMove = 70, maxDragToMove = 250, maxPressTime = 0.15f, minPlayerSpeed = 5, moveReactionTime = 0.3f, turnReactionTime = 2.5f, globalPlayerSpeed;
    public float gizmoSphereSize = 0.2f;
    public Transform root, lFoot, rFoot;
    [HideInInspector] public Vector3 rootVel, lFootVel, rFootVel;

    // --- Private
    private Vector3 rootPrePos, lFootPrePos, rFootPrePos;
    private Vector3 direction;
    private float playerSpeedInterval, timeAtTouchDown, distanceTravelled;
    private float joyDisplacementAngle = -0.25f * Mathf.PI; // This converts radians, turning by 45 degrees for isometric view.
    private Vector2 joyAnchor;
    private Vector3 oldPos;
    private bool canMove;
    private bool mouseDown;
    private Vector3 moveVector;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        preProcessing = GetComponent<MMPreProcessing>();
        trajectoryPoints = new TrajectoryPoint[preProcessing.trajectoryPointsToUse];
        InitArray(trajectoryPoints);
        playerSpeedInterval = (maxPlayerSpeed / maxDragToMove) * 100;
        oldPos = transform.position;
        rb.velocity = new Vector3(0,0,0);
    }

    /* Use velocity and acceleration to predict the points (over time) where the character will be
     * this can be done using the explicit euler method (check unity implementation in different project)
     * Once you can predict the curve over time (use changeable variables), save the points positions and forwards
     * and send them to MM for comparison
     */

    private void Update()
    {
        trajectoryPoints = CalculateTrajectory((float)preProcessing.frameStepSize/100);
    }

    private void FixedUpdate()
    {
        HandleInput();
        UpdateJointVelocities();
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

    public MMPose GetMovementPose()
    {
        return new MMPose(root.position,lFoot.position,rFoot.position,rootVel,lFootVel,rFootVel);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + direction * 2 / direction.magnitude);
            for (int i = 0; i < trajectoryPoints.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(trajectoryPoints[i].position, gizmoSphereSize);
                Gizmos.DrawLine(trajectoryPoints[i].position, trajectoryPoints[i].position + trajectoryPoints[i].forward);
            }
        }
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

                point[i].position = point[i - 1].position + Quaternion.Slerp(transform.rotation, lookRotation, timeStep * i ) * Vector3.forward;
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
    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            joyAnchor = Input.mousePosition;
            timeAtTouchDown = Time.time;
            mouseDown = true;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 joyDragVector = GetJoyDragVector(joyAnchor, Input.mousePosition);
            if (joyDragVector.magnitude > minDragToMove || Time.time - timeAtTouchDown > maxPressTime)
            {
                canMove = true;
            }
            if (canMove)
            {
                MovePlayer(new Vector3(joyDragVector.x, 0, joyDragVector.y));
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (mouseDown)
            {
                if (!canMove || Time.time - timeAtTouchDown < maxPressTime)
                {
                    // Mouse click here
                }
            }
            mouseDown = false;
            canMove = false;
        }
    }


    Vector2 GetJoyDragVector(Vector2 anchor, Vector2 goal)
    {
        Vector2 rawJoy = goal - anchor;
        rawJoy = Vector2.ClampMagnitude(rawJoy, maxDragToMove);
        return rawJoy;
    }

    void MovePlayer(Vector3 moVector)
    {
        moveVector = moVector;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position + moVector), turnReactionTime * Time.deltaTime);

        Vector3 speedMove = (moVector / maxDragToMove) * maxPlayerSpeed;

        if (speedMove.magnitude < minPlayerSpeed)
        {
            speedMove = speedMove.normalized * minPlayerSpeed;
        }

        speedMove.y = rb.velocity.y;

        rb.velocity = speedMove;

        distanceTravelled = (oldPos - transform.position).magnitude;
        oldPos = transform.position;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
