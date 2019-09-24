using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUpdater : MonoBehaviour
{
    private int maxSlots;
    private GameObject[] slot;
    public GameObject slotHolder;
    // Start is called before the first frame update
    void Start()
    {
        
        maxSlots = 40;
        slot = new GameObject[maxSlots];

        // This should get all of the slots in the inventory window. 
        for(int i = 0; i < maxSlots; i++) {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToSlot() {
        // Add the Scriptable object to the inventory slot. Called from Interaction.
        Debug.Log("Entered AddItemToSlot");
        gameObject.GetComponent<ClueObject>();
        for(int i = 0; i < maxSlots; i++) {
           /*  if(slot[i].GetComponent<Interactable>() != null) {
                Debug.Log("Entered slot if statement" + slot[i].GetComponent<ClueObject>());
                // Add clue to the slot.
                // This could be done by placing a scaled sprite in the spot of the item. 

            } */
        }
    }
}
