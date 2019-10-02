using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #if UNITY_EDITOR
[ExecuteInEditMode]
public class ID_Assigner : MonoBehaviour
{
    public bool notAssigned = false;
    // private int interactableAmount;

    private void Awake()
    {
        AssignIDsInteractables();           
    }


    //private void Update()
    //{
    //    if (Application.isEditor)
    //    {
    //        if (GameObject.FindGameObjectsWithTag("interactable") != null)
    //        {

    //            if (notAssigned == true)
    //            {
    //                AssignIDsInteractables();
    //                // interactableAmount = GameObject.FindGameObjectsWithTag("interactable").Length;
    //                // PlayerPrefs.SetInt("interactAmount", interactableAmount);
    //                Debug.Log("Interactable IDs assigned" + " " + PlayerPrefs.GetInt("interactAmount"));
    //            }

    //            //if (interactableAmount != GameObject.FindGameObjectsWithTag("interactable").Length)
    //            //{
    //            //    notAssigned = true;
    //            //    interactableAmount = PlayerPrefs.GetInt("interactAmount");
    //            //}
    //        }
    //    }
    //}

    private void AssignIDsInteractables()
    {
        if (GameObject.FindGameObjectsWithTag("interactable") == null)
            return;

        GameObject[] interactables = GameObject.FindGameObjectsWithTag("interactable");

        for (int i = 0; i < interactables.Length; i++)
        {
            Interactable tempIntScript = interactables[i].GetComponent<Interactable>();

            tempIntScript.iD = i;
        }

        Debug.Log("Interactable IDs assigned");
        notAssigned = false;
    }

}
// #endif