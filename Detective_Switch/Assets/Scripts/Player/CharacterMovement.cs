using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    private Rigidbody playerRB;
    public float maxPlayerSpeed = 8.5f, minDragToMove = 70, maxDragToMove = 250, maxPressTime = 0.15f, minPlayerSpeed = 5, moveReactionTime = 0.3f, turnReactionTime = 2.5f, globalPlayerSpeed;
    private Vector2 joyAnchor;
    private Vector3 oldPos;
    private bool canMove;
    private bool mouseDown;

    public float interactDistance = 4.0f;

    //private Quaternion lookAtPosition;
    
    private float joyDisplacementAngle = -0.25f * Mathf.PI; // This converts radians, turning by 45 degrees for isometric view.
    private float playerSpeedInterval, timeAtTouchDown, distanceTravelled;
    // Start is called before the first frame update
    void Start()
    {
        //lookAtPosition = Quaternion.LookRotation(transform.position + new Vector3(1000, 0, 0));
        playerRB = GetComponent<Rigidbody>();
        playerSpeedInterval = (maxPlayerSpeed / maxDragToMove) * 100;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //playerRB.AddForce();
        if (!GameMaster.instance.GetMenuIsOpen())
        {
            HandleInput();
            //transform.rotation = Quaternion.Lerp(transform.rotation, lookAtPosition, turnReactionTime * Time.deltaTime);
        }
    }

    void HandleInput() {
        if(Input.GetMouseButtonDown(0)) {
            //Debug.Log("Mouse button pressed");
            joyAnchor = Input.mousePosition;
            //print(joyAnchor);
            timeAtTouchDown = Time.time;
            mouseDown = true;
        }
        if(Input.GetMouseButton(0)) {
            Vector2 joyDragVector = GetJoyDragVector(joyAnchor, Input.mousePosition);
            //Vector2 joyDragVector = GetJoyDragVector(new Vector2(Screen.width/2, Screen.height/2), Input.mousePosition);
            //print(joyDragVector);
            if(joyDragVector.magnitude > minDragToMove || Time.time - timeAtTouchDown > maxPressTime) {
                canMove = true;
            }
            if(canMove) {
                //Debug.Log("I am the move");
                MovePlayer(new Vector3(joyDragVector.x, 0, joyDragVector.y));
            }
        }
        if(Input.GetMouseButtonUp(0)) {
            if (mouseDown)
            {
                if (!canMove || Time.time - timeAtTouchDown < maxPressTime)
                {
                    MouseClick();
                } else
                {
                    //lookAtPosition = transform.rotation;
                }
            }
            mouseDown = false;
            canMove = false;
        }
    }

    void MouseClick() {
        //Debug.Log("MOUSE CLICK");
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

    void MovePlayer(Vector3 moVector) {

        /*Vector3 speedMove = (moVector / maxDragToMove) * maxPlayerSpeed;
        if(speedMove.magnitude < minPlayerSpeed) {
            speedMove = speedMove.normalized * minPlayerSpeed;
        }

        playerRB.velocity = Vector3.Lerp(playerRB.velocity, speedMove * 10, moveReactionTime * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(speedMove), turnReactionTime * Time.deltaTime);

        distanceTravelled = (oldPos - transform.position).magnitude;
        oldPos = transform.position;
        GameMaster.instance.SetMoveSpeed(globalPlayerSpeed);*/


        //lookAtPosition = Quaternion.LookRotation(transform.position + moVector);

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
}
