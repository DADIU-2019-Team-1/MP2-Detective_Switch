using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Interactable : MonoBehaviour
{
    // main
    [HideInInspector]
    public int iD;
    [HideInInspector]
    public bool singleUse;
    [HideInInspector]
    public bool hasBeenClicked;

    // sound
    [HideInInspector]
    public bool soundOnInteract;
    [HideInInspector]
    public string playSound;

    // rotation
    [HideInInspector]
    public bool rotateOnInteract;
    [HideInInspector]
    public int rotateDegreesX = 0;
    [HideInInspector]
    public int rotateDegreesY = 90;
    [HideInInspector]
    public int rotateDegreesZ = 0;
    [HideInInspector]
    public bool rotateOverTime;
    [HideInInspector]
    public float rotationDuration = 0.5f;

    [HideInInspector]
    public bool isRotating;
    [HideInInspector]
    public Vector3 oldRotation;
    [HideInInspector]
    public Vector3 newRotation;
    [HideInInspector]
    public float rotationStartTime;
    [HideInInspector]
    public float rotationEndTime;

    // toggle
    [HideInInspector]
    public bool toggleGameObject;
    [HideInInspector]
    public GameObject toggleObject;
    [HideInInspector]
    public bool toggleState;

    // item
    [HideInInspector]
    public bool hasItem;

    [HideInInspector]
    public bool hasClue;
    [HideInInspector]
    public int clueKeyAmount = 0;
    [HideInInspector]
    public int[] clueKeyInt;

    [HideInInspector]
    public bool hasNote;
    [HideInInspector]
    public int noteKeyAmount = 0;
    [HideInInspector]
    public int[] noteKeyInt;

    [HideInInspector]
    public bool hasKeyItem;
    [HideInInspector]
    public Item item;

    // animation
    [HideInInspector]
    public bool hasAnimation;
    [HideInInspector]
    public bool switchBetweenAnimations;
    [HideInInspector]
    public string animationDefault;
    [HideInInspector]
    public string animationAction;
    private Animator anim;
    private bool animationState;

    // trigger
    [HideInInspector]
    public bool isTriggerOnKeyPress = false;
    [HideInInspector]
    public string triggerKey = "";
    [HideInInspector]
    public UnityEvent triggerEvent;

    // test
    [HideInInspector]
    public bool testLog;
    [HideInInspector]
    public string testLogText;

    public Vector3 Interact()
    {
        Vector3 interactResponse = new Vector3(-1, -1, -1);

        if (isRotating)
        {
            return interactResponse;
        }

        // reclickable
        if (singleUse)
        {
            if (hasBeenClicked)
            {
                return interactResponse;
            }
            hasBeenClicked = true;
        }

        // test log
        if (testLog)
        {
            Debug.Log(testLogText);
        }

        // play sound
        if (soundOnInteract)
        {
            if (!string.IsNullOrEmpty(playSound))
            {
                AkSoundEngine.PostEvent(playSound, gameObject);
            }
        }

        // rotate object
        if (rotateOnInteract)
        {
            oldRotation = transform.eulerAngles;
            newRotation = transform.eulerAngles + new Vector3(rotateDegreesX, rotateDegreesY, rotateDegreesZ);
            if (rotateOverTime)
            {
                rotationStartTime = Time.time;
                rotationEndTime = rotationStartTime + rotationDuration;
                isRotating = true;
            }
            else
            {
                transform.eulerAngles = newRotation;
            }
        }

        // toggle object
        if (toggleGameObject)
        {
            if (toggleObject != null)
            {
                toggleObject.SetActive(!toggleObject.activeSelf);
            }
            toggleState = !toggleState;
        }

        // item
        if (hasItem)
        {
            GameObject tempJournal = GameObject.FindGameObjectWithTag("Journal");

            if (hasKeyItem && item != null)
            {
                hasKeyItem = false;
                GameMaster.instance.GetComponent<InventoryUpdater>().AddItemToSlot(item);
            } 
            if(hasClue && clueKeyInt != null && clueKeyAmount != 0) {

                if (tempJournal != null)
                {
                    hasClue = false;
                    UI_Journal tempScript = tempJournal.GetComponent<UI_Journal>();
                    for (int i = 0; i < clueKeyInt.Length; i++)
                    {
                        tempScript.AddClueToJournal(tempScript.GetClue(clueKeyInt[i]));
                    }
                }
            }
            if(hasNote && noteKeyInt != null && noteKeyAmount != 0) {

                if (tempJournal != null)
                {
                    hasNote = false;
                    UI_Journal tempScript = tempJournal.GetComponent<UI_Journal>();
                    for (int i = 0; i < noteKeyInt.Length; i++)
                    {
                        tempScript.AddNoteToJournal(tempScript.GetNote(noteKeyInt[i]));
                    }
                }
            }
        }

        // animation
        if (hasAnimation)
        {
            if (switchBetweenAnimations)
            {
                anim.Play(animationState ? animationDefault : animationAction);
                animationState = !animationState;
            }
            else
            {
                anim.Play(animationAction);
            }
        }

        return gameObject.transform.position;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isRotating)
        {
            float timeStep = (Time.time - rotationStartTime) / (rotationStartTime - rotationEndTime);
            Vector3 direction = oldRotation + timeStep * (oldRotation - newRotation);
            transform.eulerAngles = direction;
            if (Time.time >= rotationEndTime || (newRotation - transform.eulerAngles).magnitude < 1)
            {
                isRotating = false;
                transform.eulerAngles = newRotation;
            }
        }
    }

    public void triggerEventInteractable()
    {
        triggerEvent.Invoke();
    }

    public void noteKeyArrayInit()
    {
        noteKeyInt = new int[noteKeyAmount];
    }

    public void clueKeyArrayInit()
    {
        clueKeyInt = new int[clueKeyAmount];
    }
}
