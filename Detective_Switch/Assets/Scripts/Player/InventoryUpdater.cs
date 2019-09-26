using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUpdater : MonoBehaviour
{
    private int maxSlots;
    private List<GameObject> slot;
    private List<GameObject> slotList;
    public GameObject slotHolder;
    public int slotIterator, slotListIterator;
    
    public GameObject itemSlotPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
        maxSlots = 5;
        slot = new List<GameObject>();
        

        
        // This should get all of the slots in the inventory window. 
        if(slotHolder != null) {
            for(int i = 0; i < maxSlots; i++) {
            // This gets the slotholder in the scene, which is the keyItemsButton.
                slot.Add(slotHolder.transform.GetChild(i).gameObject);
                //Debug.Log("Current slot is: " + slot[i]);
                // We check if the current slot has an item. If it doesn't have one, we set the empty boolean to true. 
                if(slot[i].GetComponent<Slot>().item == null) {
                    //Debug.Log(slot[i].GetComponent<Slot>().item.name);
                    slot[i].GetComponent<Slot>().empty = true;
                }
            }            
        }
        /* if(slot[0].GetComponent<Slot>() == null) {
            Debug.Log("Slot is not null: " + slot[0].GetComponent<Slot>().name);
        } */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToSlot(Item item) {
        //Debug.Log("Entered AddItemToSlot");
        // We define a new list to put our Items into.
        //AddSlotToInventory();
        List<Item> keyItemList = new List<Item>();

        keyItemList.Add(item);
        string itemName = item.name;
        int itemID = item.id;
        Sprite itemImage = item.clueImage; 
        Debug.Log(keyItemList.Count);            
        if(slotIterator >= /* slotList.Count */  maxSlots ) {
            Debug.Log("Key item inventory is full.");
            // Maybe add code here to remove the latest item if it becomes a problem. 
            return;
        }
        // Here we go through all of the indexes in the list, and check if they are empty. If they are, we can add an item to the current one.
        // It will also run the UpdateSlot() function from the Slot script, which updates the sprite of the slot in the keyItems inventory. 
        foreach(Item itemCount in keyItemList) {
            Debug.Log("List items: " + itemCount);
            //keyItemList.Add(item);
            if(slot[slotIterator].GetComponent<Slot>().empty && keyItemList.Count <= maxSlots) {
                Debug.Log("Entered add item if statement");
                //keyItemList.Add(item);
                slot[slotIterator].GetComponent<Slot>().icon = itemImage;
                slot[slotIterator].GetComponent<Slot>().id = itemID;
                slot[slotIterator].GetComponent<Slot>().text = itemName;
                slot[slotIterator].GetComponent<Slot>().UpdateSlot();
                slot[slotIterator].GetComponent<Slot>().empty = false;

                slotIterator++;
                //Debug.Log(slotIterator);
            }  

        }
        
         // Load in the item information.

        // Loop through all the slots, load in item if the slot is empty. 
/*          for(int i = 0; i < maxSlots; i++) {
             if(slot[i].GetComponent<Slot>().empty) {                
                //Debug.Log(slot[i]);
                // Debug.Log("Entered slot if statement" + slot[i].GetComponent<ClueObject>());
                Debug.Log("I am not null");
                //slot[i] = item;
                slot[i].GetComponent<Slot>().icon = item.clueImage;// Item icon
                slot[i].GetComponent<Slot>().id = itemID;
                slot[i].GetComponent<Slot>().name = itemName;

                //item.transform.parent = slot[i].transform;
                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;
                //slot[i].GetComponent<Slot>().item = item.gameObject;
                break;
                // This could be done by placing a scaled sprite in the spot of the item. 
                
            } 
             
        } */  
    }

    public void RemoveFromList() {

    }

    public void AddSlotToInventory() {
        // Make a public list, so menuInteraction can also access length to show/hide children.

        // Get the Rect transform of the SlotHolder, then instantiate at +30 Y position from that, 
        // if something is removed, - everything but slotholder and List[0] by 30.

/*         foreach(GameObject slotPos in slotList) {
            var newSlotPos = Instantiate(itemSlotPrefab, new Vector3(slotList[slotListIterator].transform.position.x, 
            slotList[slotListIterator].transform.position.y +200, slotList[slotListIterator].transform.position.z)
            , Quaternion.identity, slotHolder.transform);
            slotListIterator++;
        } */

    }
}
