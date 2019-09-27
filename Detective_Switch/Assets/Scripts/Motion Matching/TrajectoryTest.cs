using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrajectoryTest : MonoBehaviour
{
    // TODO: Get the trajectory (x points) based on the movement, then pass it to the MM class

    // --- References
    Rigidbody rb;
    [HideInInspector] public List<TrajectoryPoint> trajectoryPoints;
    MM mm;

    // --- Public
    public float[] trajPoints;
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mm = GetComponent<MM>();
        trajectoryPoints = new List<TrajectoryPoint>();
        playerSpeedInterval = (maxPlayerSpeed / maxDragToMove) * 100;
        oldPos = transform.position;
    }

    /* Use velocity and acceleration to predict the points (over time) where the character will be
     * this can be done using the explicit euler method (check unity implementation in different project)
     * Once you can predict the curve over time (use changeable variables), save the points positions and forwards
     * and send them to MM for comparison
     */

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        HandleInput();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction * 2 / direction.magnitude);
        for (int i = 0; i < trajPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(transform.position + direction * trajPoints[i] / gizmoSphereSpacing, gizmoSphereSize);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward*2);
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
        Vector2 turnedJoyVector = new Vector2(0, 0);
        turnedJoyVector.x = rawJoy.x * Mathf.Cos(joyDisplacementAngle) - rawJoy.y * Mathf.Sin(joyDisplacementAngle);
        turnedJoyVector.y = rawJoy.x * Mathf.Sin(joyDisplacementAngle) + rawJoy.y * Mathf.Cos(joyDisplacementAngle);
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

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(speedMove), turnReactionTime * Time.deltaTime);
        
        distanceTravelled = (oldPos - transform.position).magnitude;
        oldPos = transform.position;
    }
}
