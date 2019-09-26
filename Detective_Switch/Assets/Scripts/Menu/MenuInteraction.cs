using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InventoryButton(Button button) {
        Debug.Log(button.name);
        Transform invChild = button.transform.GetChild(0);
        if(!invChild.gameObject.activeInHierarchy) {
            invChild.gameObject.SetActive(true);
        }
        else {
            invChild.gameObject.SetActive(false);
        }


    }

    public void CaseFileButton(Button caseFileButton) {
        Debug.Log(caseFileButton.name);
        // Code to open the casefile.
    }

    public void OptionsButton(Button optionsButton) {
        Debug.Log(optionsButton.name);
        Transform optsChild = optionsButton.transform.parent.parent.GetChild(2);
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
       /*  GameObject canvasGO = transform.GetChild(1).gameObject;
        Debug.Log(canvasGO.name);
        // Switch case made to differentiate between a game options press, where OnScreenGUI has to be turned off, and a main menu options press.
        if(canvasGO.transform.GetChild(0).gameObject.activeInHierarchy) {
            canvasGO.transform.GetChild(0).gameObject.SetActive(false);
            canvasGO.transform.GetChild(3).gameObject.SetActive(false);
            canvasGO.transform.GetChild(2).gameObject.SetActive(true);
            
            Debug.Log(canvasGO.transform.GetChild(3).gameObject.name);
        }
        
        else if(canvasGO.transform.GetChild(2).gameObject.activeInHierarchy){
            canvasGO.transform.GetChild(2).gameObject.SetActive(false);
            canvasGO.transform.GetChild(0).gameObject.SetActive(true);
        }


        else {
            canvasGO.transform.GetChild(1).gameObject.SetActive(false);
            canvasGO.transform.GetChild(2).gameObject.SetActive(true);
        } */


    }

        
        
    
 
}
