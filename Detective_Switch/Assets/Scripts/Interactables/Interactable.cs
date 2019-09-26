using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interactable : MonoBehaviour
{
    // main
    [HideInInspector]
    public bool singleUse;
    private bool hasBeenClicked;

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

    private bool isRotating;
    private Vector3 oldRotation;
    private Vector3 newRotation;
    private float rotationStartTime;
    private float rotationEndTime;

    // toggle
    [HideInInspector]
    public bool toggleGameObject;
    [HideInInspector]
    public GameObject toggleObject;
    [HideInInspector]
    public bool toggleAfterDelay;
    [HideInInspector]
    public float toggleDelay;

    // item
    [HideInInspector]
    public bool hasItem;
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

    // test
    [HideInInspector]
    public bool testLog;
    [HideInInspector]
    public string testLogText;

    public void Interact()
    {
        if (isRotating)
        {
            return;
        }

        // reclickable
        if (singleUse)
        {
            if (hasBeenClicked)
            {
                return;
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
        }

        // item
        if (hasItem)
        {
            if (hasKeyItem && item != null)
            {
                GameMaster.instance.GetComponent<InventoryUpdater>().AddItemToSlot(item);
            } 
            if(hasClue && !string.IsNullOrEmpty(clueKeyString)) {
                // Insert Jakob's load string function
            }
            if(hasNote && !string.IsNullOrEmpty(noteKeyString)) {
                // Insert load string for note function
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
}
