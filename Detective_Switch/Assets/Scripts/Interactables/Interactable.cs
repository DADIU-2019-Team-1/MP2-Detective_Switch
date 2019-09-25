using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interactable : MonoBehaviour
{
    // main
    public bool singleUse;
    private bool hasBeenClicked;

    // sound
    public bool soundOnInteract;
    public string playSound;

    // rotation
    public bool rotateOnInteract;
    public int rotateDegreesX = 0;
    public int rotateDegreesY = 90;
    public int rotateDegreesZ = 0;
    public bool rotateOverTime;
    public float rotationDuration = 0.5f;

    private bool isRotating;
    private Vector3 oldRotation;
    private Vector3 newRotation;
    private float rotationStartTime;
    private float rotationEndTime;

    // toggle
    public bool toggleGameObject;
    public GameObject toggleObject;
    public bool toggleAfterDelay;
    public float toggleDelay;

    // item
    public bool hasItem;
    public Item item;

    // animation
    public bool hasAnimation;
    public bool switchBetweenAnimations;
    public string animationDefault;
    public string animationAction;
    private Animator anim;
    private bool animationState;

    // test
    public bool testLog;
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
            if (item != null)
            {
                GameMaster.instance.GetComponent<InventoryUpdater>().AddItemToSlot(item);
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
