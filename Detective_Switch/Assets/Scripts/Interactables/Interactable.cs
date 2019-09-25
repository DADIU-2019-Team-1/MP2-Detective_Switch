using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (!Application.isEditor)
        {
            return;
        }

        var dis = target as Interactable;

        DrawUILine();

        dis.singleUse = GUILayout.Toggle(dis.singleUse, "Single Use");

        DrawUILine();

        dis.soundOnInteract = GUILayout.Toggle(dis.soundOnInteract, "Sound On Interact");
        if (dis.soundOnInteract)
            dis.playSound = EditorGUILayout.TextField("Sound Name:", dis.playSound);

        DrawUILine();

        dis.rotateOnInteract = GUILayout.Toggle(dis.rotateOnInteract, "Rotate On Interact");
        if (dis.rotateOnInteract)
        {
            dis.rotateDegreesX = EditorGUILayout.IntSlider("Rotation X Step:", dis.rotateDegreesX, -180, 180);
            dis.rotateDegreesY = EditorGUILayout.IntSlider("Rotation Y Step:", dis.rotateDegreesY, -180, 180);
            dis.rotateDegreesZ = EditorGUILayout.IntSlider("Rotation Z Step:", dis.rotateDegreesZ, -180, 180);

            dis.rotateOverTime = GUILayout.Toggle(dis.rotateOverTime, "Rotate For Duration");
            if (dis.rotateOverTime)
                dis.rotationDuration = EditorGUILayout.Slider(dis.rotationDuration, 0.0f, 5.0f);
        }

        DrawUILine();

        dis.toggleGameObject = GUILayout.Toggle(dis.toggleGameObject, "Toggle Game Object");
        if (dis.toggleGameObject)
            dis.toggleObject = (GameObject)EditorGUILayout.ObjectField("Toggle Object:", dis.toggleObject, typeof(GameObject), true);

        DrawUILine();

        dis.hasItem = GUILayout.Toggle(dis.hasItem, "Has Item");
        if (dis.hasItem)
            dis.item = (Item)EditorGUILayout.ObjectField("Item:", dis.item, typeof(Item), true);

        DrawUILine();

        dis.hasAnimation = GUILayout.Toggle(dis.hasAnimation, "Has Animation");
        if (dis.hasAnimation)
        {
            GUILayout.Label("Object must have an animator component");
            dis.switchBetweenAnimations = GUILayout.Toggle(dis.switchBetweenAnimations, "Switch between animations");
            if (dis.switchBetweenAnimations)
                dis.animationDefault = EditorGUILayout.TextField("Animation Default:", dis.animationDefault);
            dis.animationAction = EditorGUILayout.TextField("Animation Action:", dis.animationAction);
        }

        DrawUILine();

        dis.testLog = GUILayout.Toggle(dis.testLog, "Debug");
        if (dis.testLog)
            dis.testLogText = EditorGUILayout.TextField("Text:", dis.testLogText);

        DrawUILine();
    }

    public static void DrawUILine()
    {
        Color color = new Color(1, 1, 1, 0.3f);
        int thickness = 1;
        int padding = 8;

        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}
