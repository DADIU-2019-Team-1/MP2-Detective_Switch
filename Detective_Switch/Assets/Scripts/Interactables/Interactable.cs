using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string soundName;
    public string testname;

    protected abstract void Action();
}
