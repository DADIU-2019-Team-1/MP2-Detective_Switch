using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemInteractions : MonoBehaviour
{
    public GameObject[] interactables;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void keyPressed( key or maybe using Button keyItemButton, and reference with that. ) {
        interactables = GameObject.FindGameObjectsWithTag("interactable");
          foreach(GameObject interactable in interactables) {
              Interactable tempScript = interactable.GetComponent<Interactable>();
                if(tempScript.isTriggerOnKeyPress)
                    if(closeEnough)
                        if(interactable.triggerKey = key)
                            interactable.trigger();
        
    } */
}
