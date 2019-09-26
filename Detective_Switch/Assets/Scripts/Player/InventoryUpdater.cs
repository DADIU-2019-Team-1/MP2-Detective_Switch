using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUpdater : MonoBehaviour
{
    private int maxSlots;
    private GameObject[] slot;
    //public GameObject slotHolder;
    // Start is called before the first frame update
    void Start()
    {
        
        maxSlots = 40;
        slot = new GameObject[maxSlots];

        // This should get all of the slots in the inventory window. 
        for(int i = 0; i < maxSlots; i++) {
            // Tweak this one after where the slots are placed
            //slot[i] = slotHolder.transform.GetChild(i).gameObject;
            if(slot[i].GetComponent<Slot>().item == null) {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToSlot(Item item) {
        Debug.Log("Entered AddItemToSlot");
        
        // Load in the item information.
        string itemName = item.name;
        string itemType = item.type;
        string itemText = item.text;
        int itemID = item.id;
        Sprite itemImage = item.clueImage; 
        // Loop through all the slots, load in item if the slot is empty. 
        for(int i = 0; i < maxSlots; i++) {
             if(slot[i].GetComponent<Slot>().empty) {

                
                //Debug.Log(slot[i]);
                // Debug.Log("Entered slot if statement" + slot[i].GetComponent<ClueObject>());
                Debug.Log("I am not null");
                //slot[i] = item;
                slot[i].GetComponent<Slot>().icon = item.clueImage;// Item icon
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().id = itemID;
                slot[i].GetComponent<Slot>().name = itemName;
                slot[i].GetComponent<Slot>().text = itemText;

                //item.transform.parent = slot[i].transform;
                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;
                //slot[i].GetComponent<Slot>().item = item.gameObject;
                break;
                // This could be done by placing a scaled sprite in the spot of the item. 
                
            } 
             
        }
    }
}
