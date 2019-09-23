using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    private Rigidbody playerRB;
    public float maxPlayerSpeed = 25, minDragToMove = 70, maxDragToMove = 500, maxPressTime = 0.5f;
    private Vector2 joyAnchor;
    private bool canMove;
    
    private float joyDisplacementAngle = -0.25f * Mathf.PI; // This converts radians, turning by 45 degrees for isometric view.
    private float playerSpeedInterval, timeAtTouchDown;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerSpeedInterval = (maxPlayerSpeed / maxDragToMove) * 100;

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
        }
        if(Input.GetMouseButton(0)) {
            Vector2 joyDragVector = GetJoyDragVector(joyAnchor, Input.mousePosition);
            //print(joyDragVector);
            if(joyDragVector.magnitude > minDragToMove) {
                canMove = true;
            }
            if(canMove) {
                //Debug.Log("I am the move");
                MovePlayer(new Vector3(joyDragVector.x, 0, joyDragVector.y));
            }
        }
        if(Input.GetMouseButtonUp(0)) {
            if (!canMove || Time.time - timeAtTouchDown < maxPressTime) {
                MouseClick();
            }
            canMove = false;
        }
    }

    void MouseClick() {
        Debug.Log("MOUSE CLICK");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Interactable");

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
            //int rayShift = 1 << 4;
            hit.transform.gameObject.SetActive(false);
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
        playerRB.AddForce(moVector * playerSpeedInterval * Time.deltaTime);
        print("Move vector is: " + moVector);
    }
}
