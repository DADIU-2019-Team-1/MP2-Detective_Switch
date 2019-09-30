using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class ID_Assigner : MonoBehaviour
{
    public bool assigned = false;
    int interactableAmount = 0;

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("interactable") != null)
        {
            if (assigned == false)
            {
                AssignIDsInteractables();
                Debug.Log("This is working");
            }

            if (interactableAmount != GameObject.FindGameObjectsWithTag("interactable").Length)
            {
                assigned = false;
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

        interactableAmount = interactables.Length;
        assigned = true;
    }

}
#endif