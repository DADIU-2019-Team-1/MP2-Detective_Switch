using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    private int childIterator = 0;
    private InventoryUpdater _invUpdate;
    // Start is called before the first frame update
    void Start()
    {
        _invUpdate = GameMaster.instance.GetComponent<InventoryUpdater>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InventoryButton(Button button) {
        Debug.Log(button.name);
        Transform invChild = button.transform.GetChild(0);
        
        //childIterator = 0;

        // Have a slotList.count instead of 5 here.
        if(!invChild.gameObject.activeInHierarchy && button.transform.childCount <=  10 /*  _invUpdate.slotList.Count */) {
            /* childIterator = 0;
            foreach(Transform child in transform) {
                button.transform.GetChild(childIterator).gameObject.SetActive(true);
                childIterator++;
            } */
            for(int i = 0; i < button.transform.childCount; i++) {
                button.transform.GetChild(i).gameObject.SetActive(true);
            } 
            //invChild.gameObject.SetActive(true);
        }
        else {            
            
            for(int i = 0; i < button.transform.childCount; i++) {
                //Debug.Log("Childcount is: " + button.transform.childCount);
                button.transform.GetChild(i).gameObject.SetActive(false);                
            }
 

            
            /* childIterator = 0;
            foreach(Transform child in transform) {
                button.transform.GetChild(childIterator).gameObject.SetActive(false);
                childIterator++; 
            } */

        }


    }

    public void CaseFileButton(Button caseFileButton) {
        GameObject journalRef = GameMaster.instance.FindObjectFromParentName("UI Object", "Journal");
        if(!journalRef.activeInHierarchy)
            journalRef.SetActive(true);

        //Debug.Log(caseFileButton.name);
        // Code to open the casefile.
    }

    public void OptionsButton(Button optionsButton) {
        Debug.Log(optionsButton.name);
        GameObject optsChild = GameMaster.instance.FindObjectFromParentName("MainUI", "Options");
        if(!optsChild.gameObject.activeInHierarchy) {
            optsChild.gameObject.SetActive(true);
            RemoveMenuButtons();
            
        }
    }

    public void FlashlightButton(Button flashLightButton) {
        Debug.Log(flashLightButton.name);
        Transform flashLightSwitch = flashLightButton.transform.parent;
        if(flashLightSwitch.GetChild(0).gameObject.activeInHierarchy) {
            flashLightSwitch.GetChild(0).gameObject.SetActive(false);
            flashLightSwitch.GetChild(1).gameObject.SetActive(true);
        }
        else {
            flashLightSwitch.GetChild(0).gameObject.SetActive(true);
            flashLightSwitch.GetChild(1).gameObject.SetActive(false);
        }
    }

    // Make it switch between the 2 different buttons, like flashlight.
    public void LocalizationButton(Button localizationButton) {
        Transform localizationSwitch = localizationButton.transform.parent;
        if(localizationSwitch.GetChild(0).gameObject.activeInHierarchy) {
            localizationSwitch.GetChild(0).gameObject.SetActive(false);
            localizationSwitch.GetChild(1).gameObject.SetActive(true);
        }
        else {
            localizationSwitch.GetChild(0).gameObject.SetActive(true);
            localizationSwitch.GetChild(1).gameObject.SetActive(false);
        }
    }


    // Update RemoveMenuButtons with Sebastian's code to get child from last production.
    public void RemoveMenuButtons() {
        GameObject canvasGO = GameMaster.instance.FindObjectFromParentName("MainUI", "OnScreenUI"); //transform.GetChild(1).gameObject;
        Debug.Log(canvasGO.name);
        // Switch case made to differentiate between a game options press, where OnScreenGUI has to be turned off, and a main menu options press.
        if(canvasGO.gameObject.activeInHierarchy) {
            canvasGO.SetActive(false);
            /* canvasGO.transform.GetChild(0).gameObject.SetActive(false);
            canvasGO.transform.GetChild(3).gameObject.SetActive(false);
            canvasGO.transform.GetChild(2).gameObject.SetActive(true);
            
            Debug.Log(canvasGO.transform.GetChild(3).gameObject.name); */
        }
        
        else if(GameMaster.instance.FindObjectFromParentName("MainUI", "Options").gameObject.activeInHierarchy){
            GameMaster.instance.FindObjectFromParentName("MainUI", "Options").SetActive(false);
            canvasGO.SetActive(true);
        }


        else {
            canvasGO.transform.GetChild(1).gameObject.SetActive(false);
            canvasGO.transform.GetChild(2).gameObject.SetActive(true);
        } 


    }

        
        
    
 
}
