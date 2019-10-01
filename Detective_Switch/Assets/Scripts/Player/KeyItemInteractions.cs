using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyItemInteractions : MonoBehaviour
{
    private GameObject player;
    private float minDistanceInteract = 4;
    public GameObject[] interactables;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void keyPressed(Button keyItemButton) {
        interactables = GameObject.FindGameObjectsWithTag("interactable");
            foreach(GameObject interactable in interactables) {
              Interactable tempScript = interactable.GetComponent<Interactable>();
                if(tempScript.isTriggerOnKeyPress)
                    if(CheckDistance(interactable))
                        if(tempScript.triggerKey == keyItemButton.GetComponent<Slot>().text) {
                            //Debug.Log("trigger event if");
                            keyItemButton.GetComponent<Slot>().icon = keyItemButton.GetComponent<Slot>().greyedOutImage;
                            keyItemButton.GetComponent<Slot>().UpdateSlot();
                            
                            
                            tempScript.triggerEventInteractable();

                        }

        
            } 
    }

    bool CheckDistance(GameObject thisInteractable) {
        bool isCloseEnough = false;
        Vector3 playerPos = player.transform.position;
        //Debug.Log("Player is: " + player.name);
        Vector3 interactablePos = thisInteractable.transform.position;
        if(Vector3.Distance(playerPos, interactablePos) < minDistanceInteract) {
            //Debug.Log(Vector3.Distance(playerPos, interactablePos));
            isCloseEnough = true;
            //Debug.Log(isCloseEnough);
        }
        return isCloseEnough;
    }
}
