using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrajectoryTest : MonoBehaviour
{
    // TODO: Get the trajectory (x points) based on the movement, then pass it to the MM class

    // --- References
    Rigidbody rb;
    [HideInInspector] public TrajectoryPoint[] trajectoryPoints;
    MMPreProcessing preProcessing;

    // --- Public
    public float maxPlayerSpeed = 8.5f, minDragToMove = 70, maxDragToMove = 250, maxPressTime = 0.15f, minPlayerSpeed = 5, moveReactionTime = 0.3f, turnReactionTime = 2.5f, globalPlayerSpeed;
    public float gizmoSphereSize = 0.2f, gizmoSphereSpacing = 50;

    // --- Private
    Vector3 direction;
    private float playerSpeedInterval, timeAtTouchDown, distanceTravelled;
    private float joyDisplacementAngle = -0.25f * Mathf.PI; // This converts radians, turning by 45 degrees for isometric view.
    private Vector2 joyAnchor;
    private Vector3 oldPos;
    private bool canMove;
    private bool mouseDown;

    public float acceleration;
    float mass = 5;
    float spring;

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
        //Debug.Log("Rb velocity: " + rb.velocity);
        //for (int i = 0; i < trajectoryPoints.Length; i++)
        //{
        //    Debug.Log("Trajectory point " + i + " pos: " + trajectoryPoints[i].position);
        //    Debug.Log("Trajectory point " + i + " fw: " + trajectoryPoints[i].forward);
        //}
    }

    private void FixedUpdate()
    {
        HandleInput();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction * 2 / direction.magnitude);
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(trajectoryPoints[i].position, gizmoSphereSize);
            Gizmos.DrawLine(trajectoryPoints[i].position, trajectoryPoints[i].position + trajectoryPoints[i].forward);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward*2);
    }

    TrajectoryPoint[] CalculateTrajectory(float timeStep)
    {
        TrajectoryPoint[] point = new TrajectoryPoint[trajectoryPoints.Length];
        InitArray(point);
        Vector3 velAtPreviousPos = rb.velocity;
        Vector3 velAtNextPos = rb.velocity;
        for (int i = 0; i < point.Length; i++)
        {
            // add trajectory curve points to array based on velocity and acceleration DONE
            // Acceleration = change in velocity over time (deltaVel /deltaTime)
            // TODO: Approximate with explicit euler method (Completed - verify?)
            // TODO: Handle reset when there is no player input (points north when last dir was south)

            if (i != 0)
            {
                point[i].position = point[i-1].position + rb.velocity * (timeStep * i);
                velAtNextPos += velAtNextPos * (timeStep * i); // timeStep e.g. 0.25f
                point[i].forward = point[i].position + (velAtNextPos - velAtPreviousPos) / (timeStep * i); // pos + acceleration
                velAtPreviousPos = velAtNextPos;
            }
            else
            {
                point[i].position = transform.position;
                point[i].forward = point[i].position + rb.velocity;
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
        Vector2 turnedJoyVector = new Vector2(0, 0)
        {
            x = rawJoy.x * Mathf.Cos(joyDisplacementAngle) - rawJoy.y * Mathf.Sin(joyDisplacementAngle),
            y = rawJoy.x * Mathf.Sin(joyDisplacementAngle) + rawJoy.y * Mathf.Cos(joyDisplacementAngle)
        };
        turnedJoyVector = Vector2.ClampMagnitude(turnedJoyVector, maxDragToMove);
        return turnedJoyVector;
    }

    void MovePlayer(Vector3 moVector)
    {
        direction = moVector;
        Vector3 speedMove = (moVector / maxDragToMove) * maxPlayerSpeed;
        if (speedMove.magnitude < minPlayerSpeed)
        {
            speedMove = speedMove.normalized * minPlayerSpeed;
        }
        rb.velocity = Vector3.Lerp(rb.velocity, speedMove * 10, moveReactionTime * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity), turnReactionTime * Time.deltaTime);
        
        distanceTravelled = (oldPos - transform.position).magnitude;
        oldPos = transform.position;
    }
}
