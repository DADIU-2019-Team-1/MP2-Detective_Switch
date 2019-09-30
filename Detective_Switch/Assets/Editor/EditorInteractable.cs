using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
// [ExecuteInEditMode]

#if UNITY_EDITOR

using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        var dis = target as Interactable;

        dis.iD = EditorGUILayout.IntField("Unique ID", dis.iD);

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
        if (dis.hasItem) {
            dis.hasClue = GUILayout.Toggle(dis.hasClue, "Has Clue");
            if (dis.hasClue)
            {
                dis.clueKeyAmount = EditorGUILayout.IntField("Key amount:", dis.clueKeyAmount);
                for (int i = 0; i < dis.clueKeyInt.Length; i++)
                {
                    dis.clueKeyInt[i] = EditorGUILayout.IntField("Clue Key: ", dis.clueKeyInt[i]);
                }
            }   
            dis.hasNote = GUILayout.Toggle(dis.hasNote, "Has Note");
            if(dis.hasNote)
            {
                dis.noteKeyAmount = EditorGUILayout.IntField("Key amount:", dis.noteKeyAmount);
                for (int i = 0; i < dis.noteKeyInt.Length; i++)
                {
                    dis.noteKeyInt[i] = EditorGUILayout.IntField("Note Key: ", dis.noteKeyInt[i]);
                }
            }
            dis.hasKeyItem = GUILayout.Toggle(dis.hasKeyItem, "Has KeyItem");
            if(dis.hasKeyItem) 
                dis.item = (Item)EditorGUILayout.ObjectField("Item:", dis.item, typeof(Item), true);
        }
  

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

        dis.isTriggerOnKeyPress = GUILayout.Toggle(dis.isTriggerOnKeyPress, "Is trigger on key press?");
        dis.triggerKey = EditorGUILayout.TextField("Trigger Key:", dis.triggerKey);
        SerializedProperty eventProp = serializedObject.FindProperty("triggerEvent");
        EditorGUILayout.PropertyField(eventProp);

        DrawUILine();

        dis.testLog = GUILayout.Toggle(dis.testLog, "Debug");
        if (dis.testLog)
            dis.testLogText = EditorGUILayout.TextField("Text:", dis.testLogText);

        DrawUILine();

        if (dis.clueKeyAmount != dis.clueKeyInt.Length)
        {
            dis.clueKeyInt = new int[dis.clueKeyAmount];
        }

        if (dis.noteKeyAmount != dis.noteKeyInt.Length)
        {
            dis.noteKeyInt = new int[dis.noteKeyAmount];
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(dis);
        }

        serializedObject.ApplyModifiedProperties();
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
#endif