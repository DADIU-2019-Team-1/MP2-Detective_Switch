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
    
    private float joyDisplacementAngle = -0.25f * Mathf.PI; // This converts radians, turning by 45 degrees for isometric view.
    private float playerSpeedInterval, timeAtTouchDown, distanceTravelled;
    [SerializeField]
    private InventoryUpdater _invUpdater;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerSpeedInterval = (maxPlayerSpeed / maxDragToMove) * 100;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //playerRB.AddForce();
        HandleInput();
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
        LayerMask mask = LayerMask.GetMask("Interactable");

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
            hit.transform.gameObject.GetComponent<Interactable>().Interact();
            //_invUpdater.AddItemToSlot();
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
/*         Vector3 poelse = moVector;
        if(poelse.magnitude  * playerSpeedInterval < minPlayerSpeed) {
            poelse = moVector.normalized * minPlayerSpeed;
        }
        print("Can move: " + canMove + ", speed" + poelse.magnitude);
        playerRB.AddForce(poelse * playerSpeedInterval * Time.deltaTime); */
        //print("Move vector is: " + moVector);
        Vector3 speedMove = (moVector / maxDragToMove) * maxPlayerSpeed;
        if(speedMove.magnitude < minPlayerSpeed) {
            speedMove = speedMove.normalized * minPlayerSpeed;
        }
        //playerRB.AddForce(speedMove * Time.deltaTime * 100);
        playerRB.velocity = Vector3.Lerp(playerRB.velocity, speedMove * 10, moveReactionTime * Time.deltaTime);
        //print(speedMove);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(speedMove), turnReactionTime * Time.deltaTime);
        //globalPlayerSpeed = playerRB.velocity.magnitude;
        
        distanceTravelled = (oldPos - transform.position).magnitude;
        oldPos = transform.position;
        GameMaster.instance.SetMoveSpeed(globalPlayerSpeed);
        //Debug.Log(distanceTravelled);
        //Debug.Log(globalPlayerSpeed);
    }
}
