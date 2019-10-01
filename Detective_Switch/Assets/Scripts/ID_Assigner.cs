using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class ID_Assigner : MonoBehaviour
{
    public bool notAssigned = false;
    private int interactableAmount;

    private void Awake()
    {
        if (Application.isEditor)
        {
            interactableAmount = PlayerPrefs.GetInt("interactAmount");

            if (PlayerPrefs.GetInt("interactAmount") == 0)
            {
                notAssigned = true;
            }
        }

    }
    private void Update()
    {
        if (Application.isEditor)
        {
            if (GameObject.FindGameObjectsWithTag("interactable") != null || GameObject.FindGameObjectsWithTag("interactable").Length != 0)
            {

                if (notAssigned == true)
                {
                    AssignIDsInteractables();
                    interactableAmount = GameObject.FindGameObjectsWithTag("interactable").Length;
                    PlayerPrefs.SetInt("interactAmount", interactableAmount);
                    Debug.Log("Interactable IDs assigned" + " " + PlayerPrefs.GetInt("interactAmount"));
                }

                if (interactableAmount != GameObject.FindGameObjectsWithTag("interactable").Length)
                {
                    notAssigned = true;
                    interactableAmount = PlayerPrefs.GetInt("interactAmount");
                }
            }
        }
    }

    private void AssignIDsInteractables()
    {
        if (GameObject.FindGameObjectsWithTag("interactable") == null)
            return;

        GameObject[] interactables = GameObject.FindGameObjectsWithTag("interactable");

        for (int i = 0; i < interactables.Length; i++)
        {
            InteractableObjectContainer tempIntObjCon = new InteractableObjectContainer();
            Interactable tempIntScript = interactables[i].GetComponent<Interactable>();

            tempIntScript.iD = i;
        }

        notAssigned = false;
    }

}
#endif