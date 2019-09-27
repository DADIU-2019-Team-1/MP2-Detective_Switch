using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class SaveLoadSystem : MonoBehaviour
{
    public bool firstRun = false;
    public string saveLocation = "Assets/Resources/SaveFiles/";
    public string saveFileName = "save1.csv";
    private string fullSavePath = "";

    private void Awake()
    {
        /*
        if (PlayerPrefs.GetInt("firstRun") == 1)
        {
            
        }
        else
        {
            firstRun = true;
            PlayerPrefs.SetInt("firstRun", 1);
        } */

        fullSavePath = saveLocation + saveFileName;
    }

    private void Start()
    {
        // SaveGame();
    }

    public void LoadGame(string saveName)
    {
        fullSavePath = saveLocation + saveName + ".csv";
    }

    public void LoadGame()
    {
        StreamReader strReader = new StreamReader(fullSavePath);

        bool endOfFile = false;
        bool firstRun = true;

        while (!endOfFile)
        {
            string dataString = strReader.ReadLine();

            if (dataString == null)
            {
                endOfFile = true;
                break;
            }

            if (firstRun)
            {
                // To ignore first line
                firstRun = false;
            }
            else
            {
                string[] tempDataValues = dataString.Split(';');
                float[] dataValues = new float[tempDataValues.Length];
                for (int i = 1; i < dataValues.Length; i++)
                {
                    dataValues[i] = float.Parse(tempDataValues[i], CultureInfo.InvariantCulture.NumberFormat);
                }

                // Do something
            }
        }
    }

    public bool GetHasRunBool()
    {
        return firstRun;
    }

    public void SaveGame()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null || GameObject.FindGameObjectsWithTag("interactable") == null)
            return;
     
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("interactable");

        // Player save:
        Vector3 playerPos = player.transform.position;
        Vector3 playerRot = player.transform.rotation.eulerAngles;

        // Interactable object save:
        List<InteractableObjectContainer> IntObjConList = new List<InteractableObjectContainer>();

        for (int i = 0; i < interactables.Length; i++)
        {
            InteractableObjectContainer tempIntObjCon = new InteractableObjectContainer();
            Interactable tempIntScript = interactables[i].GetComponent<Interactable>();

            tempIntObjCon.position = interactables[i].transform.position;
            tempIntObjCon.rotation = interactables[i].transform.rotation.eulerAngles;
            //tempIntObjCon.singleUse = tempIntScript.singleUse;
            //tempIntObjCon.soundOnInteract = tempIntScript.soundOnInteract;
            //tempIntObjCon.playSound = tempIntScript.playSound;
            //tempIntObjCon.rotateOnInteract = tempIntScript.rotateOnInteract;
            //tempIntObjCon.rotateOverTime = tempIntScript.rotateOverTime;
            //tempIntObjCon.rotationDuration = tempIntScript.rotationDuration;
            //tempIntObjCon.toggleGameObject = tempIntScript.toggleGameObject;
            //tempIntObjCon.toggleObject = tempIntScript.toggleObject;
            //tempIntObjCon.toggleAfterDelay = tempIntScript.toggleAfterDelay;
            //tempIntObjCon.toggleDelay = tempIntScript.toggleDelay;
            tempIntObjCon.hasItem = tempIntScript.hasItem;
            tempIntObjCon.hasClue = tempIntScript.hasClue;
            //tempIntObjCon.clueKeyString = tempIntScript.clueKeyString;
            tempIntObjCon.hasNote = tempIntScript.hasNote;
            //tempIntObjCon.noteKeyString = tempIntScript.noteKeyString;
            tempIntObjCon.hasKeyItem = tempIntScript.hasKeyItem;
            //tempIntObjCon.item = tempIntScript.item;
            //tempIntObjCon.hasAnimation = tempIntScript.hasAnimation;
            //tempIntObjCon.switchBetweenAnimations = tempIntScript.switchBetweenAnimations;
            //tempIntObjCon.animationDefault = tempIntScript.animationDefault;
            //tempIntObjCon.animationAction = tempIntScript.animationAction;

            IntObjConList.Add(tempIntObjCon);
        }

        string tempIntObjString = JsonUtility.ToJson(IntObjConList[1]);
        Debug.Log(tempIntObjString);

        for (int i = 0; i < IntObjConList.Count; i++)
        {

        }

    }
}

public class InteractableObjectContainer
{
    ////// Transform: //////
    public Vector3 position;
    public Vector3 rotation;

    ////// Interactable component variables: //////

    public bool hasItem;
    public bool hasClue;
    public bool hasNote;
    public bool hasKeyItem;
}
