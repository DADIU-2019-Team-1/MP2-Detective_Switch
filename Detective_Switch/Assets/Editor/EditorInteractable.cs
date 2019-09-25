using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CanEditMultipleObjects]
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