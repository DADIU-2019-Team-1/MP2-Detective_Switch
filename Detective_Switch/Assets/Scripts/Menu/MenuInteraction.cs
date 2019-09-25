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

    public void RemoveMenuButtons() {
        GameObject canvasGO = transform.GetChild(1).gameObject;
        Debug.Log(canvasGO.name);
        // Switch case made to differentiate between a game options press, where OnScreenGUI has to be turned off, and a main menu options press.
        if(canvasGO.transform.GetChild(0).gameObject.activeInHierarchy) {
            canvasGO.transform.GetChild(0).gameObject.SetActive(false);
            canvasGO.transform.GetChild(2).gameObject.SetActive(true);
            Debug.Log(canvasGO.transform.GetChild(2).gameObject.name);
        }
        else {
            canvasGO.transform.GetChild(1).gameObject.SetActive(false);
            canvasGO.transform.GetChild(2).gameObject.SetActive(true);
        }
            


    }

        
        
    
 
}
