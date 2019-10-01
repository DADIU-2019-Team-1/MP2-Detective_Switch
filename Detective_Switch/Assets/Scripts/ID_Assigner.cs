using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class ID_Assigner : MonoBehaviour
{
    public bool assigned = true;

    private void Start()
    {
        if (assigned != false)
        {
            if (GameObject.FindGameObjectsWithTag("interactable") != null)
            {
                AssignIDsInteractables();
                Debug.Log("Interactable IDs assigned");

            }
        }
    }
    //private void Update()
    //{
    //    if (GameObject.FindGameObjectsWithTag("interactable") != null)
    //    {
    //        if (assigned == false)
    //        {
    //            AssignIDsInteractables();
    //            Debug.Log("Interactable IDs assigned");
    //        }

    //        if (Input.GetKeyDown("q"))
    //        {
    //            assigned = false;
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
            InteractableObjectContainer tempIntObjCon = new InteractableObjectContainer();
            Interactable tempIntScript = interactables[i].GetComponent<Interactable>();

            tempIntScript.iD = i;
        }

        assigned = true;
    }

}
#endif