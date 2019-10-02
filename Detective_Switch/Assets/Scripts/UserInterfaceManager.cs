using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UserInterfaceManager : MonoBehaviour
{
    public ThisUISystem[] eventsUI;

    void Start()
    {
        for (int i = 0; i < eventsUI.Length; i++)
        {
            if (eventsUI[i].eventName != "" || eventsUI[i].eventToFire != null)
            {
                eventsUI[i].attachedManager = this.gameObject;
                StartClassCoroutine(i, (int)eventsUI[i].function);
            }
            else
            {
                Debug.Log("UI Manager Notice: UI event number " + i + " is not set up correctly!");
            }

        }

    }

    void Update()
    {
        // Not used atm.
    }

    public void Fire(string eventName)
    {
        bool isFound = false;

        for (int i = 0; i < eventsUI.Length; i++)
        {
            if (eventName == eventsUI[i].eventName)
            {
                isFound = true;
                StartCoroutine(eventsUI[i].ExternalFire());
            }
        }

        if (isFound == false)
        {
            Debug.LogError("UI Manager Error: " + eventName + " not found in UI events! Please note that event names are case sensitive.");
        }
    }

    void StartClassCoroutine(int eventNum, int funcNum)
    {
        switch (funcNum)
        {
            case 0:
                // ExternalFire selected, no need to run any functions
                break;
            case 1:
                //
                break;
            case 2:
                //
                break;
            case 3:
                //
                break;
            case 4:
                //
                break;
            case 5:
                //
                break;
            default:
                break;
        }
    }

}

[System.Serializable]
public class ThisUISystem
{
    [HideInInspector]
    public GameObject attachedManager;

    private bool hasFired = false;
    [HideInInspector]
    public bool activeInInspector = false;

    [HideInInspector]
    public string eventName = "";

    public enum myFuncEnum
    {
        ExternalFire,
        YYY1,
        YYY2,
        YYY3,
        YYY4,
        YYY5
    };

    [HideInInspector]
    public UnityEvent eventToFire;

    [HideInInspector]
    public myFuncEnum function;

    [HideInInspector]
    public float delayForFire = 0f;

    [HideInInspector]
    public GameObject thisGameObject;
    [HideInInspector]
    public string collisionTag = "";
    [HideInInspector]
    public bool isTrigger = true;
    [HideInInspector]
    public float fireCooldown = 0f;
    // public UnityEvent boolEvent;

    public void OnCollisionWithTag()
    {
        if (thisGameObject.GetComponent<ColliderChecker>() == null)
        {
            thisGameObject.AddComponent<ColliderChecker>();
        }

        if (thisGameObject.GetComponent<ColliderChecker>() != null)
        {

            if (hasFired)
            {
                Debug.Log(eventName + " event already fired");
            }
            else
            {
                thisGameObject.GetComponent<ColliderChecker>().SetUpColliderChecker(eventName, fireCooldown, isTrigger, attachedManager, collisionTag);
            }

        }
        else
        {
            Debug.Log(eventName + " error: ColliderChecker script missing!");
        }
    }

    public void OnCollision()
    {
        if (thisGameObject.GetComponent<ColliderChecker>() == null)
        {
            thisGameObject.AddComponent<ColliderChecker>();
        }

        if (thisGameObject.GetComponent<ColliderChecker>() != null)
        {

            if (hasFired)
            {
                Debug.Log(eventName + " event already fired");
            }
            else
            {
                thisGameObject.GetComponent<ColliderChecker>().SetUpColliderChecker(eventName, fireCooldown, isTrigger, attachedManager);
            }

        }
        else
        {
            Debug.Log(eventName + " error: ColliderChecker script missing!");
        }
    }

    public IEnumerator OnObjectDestroy()
    {
        if (hasFired)
        {
            Debug.Log(eventName + " event already fired");
        }
        else
        {
            bool loop = true;
            while (loop)
            {
                yield return new WaitForSeconds(0.1f);

                if (thisGameObject == null)
                {
                    yield return new WaitForSeconds(delayForFire);
                    loop = false;
                    eventToFire.Invoke();
                    Debug.Log(eventName + " event fired!");
                }

            }
        }
    }

    public IEnumerator ExternalFire()
    {
        yield return new WaitForSeconds(delayForFire);

        if (hasFired)
        {
            Debug.Log(eventName + " event already fired");
        }
        else
        {
            eventToFire.Invoke();
            Debug.Log(eventName + " event fired!");
            hasFired = true;
        }

        if (fireCooldown > 0f)
        {
            yield return new WaitForSeconds(fireCooldown);
            hasFired = false;
        }

    }

    public IEnumerator TimedEvent()
    {

        bool loop = true;
        while (loop)
        {
            yield return new WaitForSeconds(delayForFire);

            eventToFire.Invoke();
            Debug.Log(eventName + " event fired!");

            if (fireCooldown == 0)
            {
                loop = false;
            }

            yield return new WaitForSeconds(fireCooldown);

        }
    }

    public IEnumerator OnObjectMoving()
    {
        Vector3 tempPos = thisGameObject.transform.position;
        bool loop = true;
        while (loop)
        {
            if (thisGameObject.transform.position != tempPos)
            {
                yield return new WaitForSeconds(delayForFire);

                eventToFire.Invoke();
                Debug.Log(eventName + " event fired!");

                if (fireCooldown == 0)
                {
                    loop = false;
                }

                yield return new WaitForSeconds(fireCooldown);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UserInterfaceManager))]
public class UIManager_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("UI Event Manager - Not functional yet!", MessageType.None);
        DrawDefaultInspector(); // for other non-HideInInspector fields

        var script = target as UserInterfaceManager;

        for (int i = 0; i < script.eventsUI.Length; i++)
        {
            script.eventsUI[i].activeInInspector = EditorGUILayout.Foldout(script.eventsUI[i].activeInInspector, script.eventsUI[i].eventName);

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            if (script.eventsUI[i].activeInInspector)
            {
                script.eventsUI[i].eventName = EditorGUILayout.TextField("Event Name:", script.eventsUI[i].eventName);

                SerializedProperty functionProp = serializedObject.FindProperty("eventsUI.Array.data[" + i + "].function");
                EditorGUILayout.PropertyField(functionProp);

                script.eventsUI[i].delayForFire = EditorGUILayout.FloatField("Fire Delay", script.eventsUI[i].delayForFire);


                if ((int)script.eventsUI[i].function == 0)
                {
                    // External fire //
                    EditorGUILayout.LabelField("Fire cooldown, if 0 then can only be fired once:");
                    script.eventsUI[i].fireCooldown = EditorGUILayout.FloatField("Fire Cooldown", script.eventsUI[i].fireCooldown);
                    EditorGUILayout.HelpBox("This is an event you need to call from a script.\n" +
                        "You simply do that by calling the ExternalFire function in the EventManager script and give the specific event name as argument. " +
                        "If more events have the same name, they will also be fired. NOTICE, the names are case sensitive!", MessageType.Info);
                }
                if ((int)script.eventsUI[i].function == 1) // if bool is true, show other fields
                {
                    // Collision // 
                    EditorGUILayout.LabelField("Fire cooldown, if 0 then can only be fired once:");
                    script.eventsUI[i].fireCooldown = EditorGUILayout.FloatField("Fire Cooldown", script.eventsUI[i].fireCooldown);
                    script.eventsUI[i].isTrigger = EditorGUILayout.Toggle("Is Trigger?", script.eventsUI[i].isTrigger);
                    script.eventsUI[i].thisGameObject = EditorGUILayout.ObjectField("Collider Object", script.eventsUI[i].thisGameObject, typeof(GameObject), true) as GameObject;
                    EditorGUILayout.HelpBox("This fires an event when the selected object collides with anything. Please use 'is trigger' depending on the nature of the collision. Not fully tested yet.", MessageType.Warning);
                }
                if ((int)script.eventsUI[i].function == 2)
                {
                    // Collision with tag //
                    EditorGUILayout.LabelField("Fire cooldown, if 0 then can only be fired once:");
                    script.eventsUI[i].fireCooldown = EditorGUILayout.FloatField("Fire Cooldown", script.eventsUI[i].fireCooldown);
                    script.eventsUI[i].collisionTag = EditorGUILayout.TextField("Collision Tag Name", script.eventsUI[i].collisionTag);
                    script.eventsUI[i].isTrigger = EditorGUILayout.Toggle("Is Trigger?", script.eventsUI[i].isTrigger);
                    script.eventsUI[i].thisGameObject = EditorGUILayout.ObjectField("Collider Object", script.eventsUI[i].thisGameObject, typeof(GameObject), true) as GameObject;
                    EditorGUILayout.HelpBox("This fires an event when the selected object collides with a specific tag. NOTICE, the names are case sensitive!. Please use 'is trigger' depending on the nature of the collision. Not fully tested yet.", MessageType.Warning);
                }
                if ((int)script.eventsUI[i].function == 3)
                {
                    // Check if object destroyed //
                    script.eventsUI[i].thisGameObject = EditorGUILayout.ObjectField("GameObject", script.eventsUI[i].thisGameObject, typeof(GameObject), true) as GameObject;
                    EditorGUILayout.HelpBox("Attach the GameObject you want to listen to. When this specific object is destroyed, the event will fire.", MessageType.Info);
                }

                if ((int)script.eventsUI[i].function == 4)
                {
                    // Timed event //
                    EditorGUILayout.LabelField("Fire cooldown, if 0 then can only be fired once:");
                    script.eventsUI[i].fireCooldown = EditorGUILayout.FloatField("Fire Cooldown", script.eventsUI[i].fireCooldown);
                    EditorGUILayout.HelpBox("Specify the time interval for when the event should be fired", MessageType.Info);
                }
                if ((int)script.eventsUI[i].function == 5)
                {
                    // Object moving // 
                    script.eventsUI[i].fireCooldown = EditorGUILayout.FloatField("Fire Cooldown", script.eventsUI[i].fireCooldown);
                    script.eventsUI[i].thisGameObject = EditorGUILayout.ObjectField("GameObject", script.eventsUI[i].thisGameObject, typeof(GameObject), true) as GameObject;
                    EditorGUILayout.HelpBox("Fires an event when the selected gameobject is moving. Not fully tested yet", MessageType.Warning);
                }

                SerializedProperty fireProp = serializedObject.FindProperty("eventsUI.Array.data[" + i + "].eventToFire");
                EditorGUILayout.PropertyField(fireProp);
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }
    }
}
#endif
