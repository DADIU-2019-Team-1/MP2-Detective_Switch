using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // sound
    public bool soundOnInteract;
    public string playSound;

    // rotation
    public bool rotateOnInteract;
    public int rotateDegreesX = 0;
    public int rotateDegreesY = 90;
    public int rotateDegreesZ = 0;
    public bool rotateOverTime;
    public float rotationSpeed = 0.1f;

    private bool isRotating;
    private Vector3 oldRotation;
    private Vector3 newRotation;
    private float rotationStartTime;
    private float rotationJourneyLength;

    // toggle
    public bool toggleGameObject;
    public GameObject toggleObject;
    public bool toggleAfterDelay;
    public float toggleDelay;

    // item
    public bool hasItem;
    public GameObject item;

    public void Interact()
    {
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
            if (rotateOverTime)
            {
                Debug.Log("ROTATE OVER TIME");
                isRotating = true;
                oldRotation = transform.eulerAngles;
                newRotation = transform.eulerAngles + new Vector3(rotateDegreesX, rotateDegreesY, rotateDegreesZ);
                rotationStartTime = Time.time;
                rotationJourneyLength = Vector3.Angle(oldRotation, newRotation);
            }
            else
            {
                Debug.Log("ROTATE INSTANT");
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x + rotateDegreesX,
                    transform.eulerAngles.y + rotateDegreesY,
                    transform.eulerAngles.z + rotateDegreesZ
                );
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

        if(hasItem) {
            if(item != null) {
                GameMaster.instance.GetComponent<InventoryUpdater>().AddItemToSlot(item);
            }
        }

    }

    void Update()
    {
        if (isRotating)
        {
            /*float distCovered = (Time.time - rotationStartTime) * rotationSpeed;
            float fractionOfJourney = distCovered / rotationJourneyLength;
            transform.eulerAngles = Vector3.Lerp(oldRotation, newRotation, fractionOfJourney);*/
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, turningRate * Time.deltaTime);
            if (Vector3.Angle(oldRotation, newRotation) < 0.1f)
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
        var dis = target as Interactable;

        DrawUILine();

        dis.soundOnInteract = GUILayout.Toggle(dis.soundOnInteract, "Sound On Interact");
        if (dis.soundOnInteract)
            dis.playSound = EditorGUILayout.TextField("Sound Name:", dis.playSound);

        DrawUILine();

        dis.rotateOnInteract = GUILayout.Toggle(dis.rotateOnInteract, "Rotate On Interact");
        if (dis.rotateOnInteract)
        {
            dis.rotateDegreesX = EditorGUILayout.IntSlider("Rotation X Step:", dis.rotateDegreesX, 0, 360);
            dis.rotateDegreesY = EditorGUILayout.IntSlider("Rotation Y Step:", dis.rotateDegreesY, 0, 360);
            dis.rotateDegreesZ = EditorGUILayout.IntSlider("Rotation Z Step:", dis.rotateDegreesZ, 0, 360);

            dis.rotateOverTime = GUILayout.Toggle(dis.rotateOverTime, "Rotate Over Time");
            if (dis.rotateOverTime)
                dis.rotationSpeed = EditorGUILayout.Slider(dis.rotationSpeed, 0.0f, 5.0f);
        }

        DrawUILine();

        dis.toggleGameObject = GUILayout.Toggle(dis.toggleGameObject, "Toggle Game Object");
        if (dis.toggleGameObject)
            dis.toggleObject = (GameObject)EditorGUILayout.ObjectField("Toggle Object:", dis.toggleObject, typeof(GameObject), true);

        DrawUILine();

        dis.hasItem = GUILayout.Toggle(dis.hasItem, "Has Item");
        if (dis.hasItem)
            dis.item = (GameObject)EditorGUILayout.ObjectField("Item:", dis.item, typeof(GameObject), true);

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
