using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string soundName;
    public string testname;

    public void Interact()
    {
        Debug.Log(testname);
    }
}
