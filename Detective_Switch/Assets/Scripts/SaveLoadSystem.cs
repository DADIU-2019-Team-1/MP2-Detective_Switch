using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class SaveLoadSystem : MonoBehaviour
{
    public bool newGame = true;
    public string saveLocation = "Assets/Resources/SaveFiles/";
    // public string saveFileName = "save1.csv";
    private string fullSavePath = "";

    private void Awake()
    {

        if (newGame)
        {
            PlayerPrefs.SetInt("previousGame", 1);
        }
        else if (PlayerPrefs.GetInt("previousGame") == 1)
        {
            // Load previous game
        }
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
        return newGame;
    }

    public void SaveGame()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null || GameObject.FindGameObjectsWithTag("interactable") == null || GameObject.FindGameObjectWithTag("Journal") == null)
            return;
     
        // Find gameobjects:
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("interactable");
        GameObject journal = GameObject.FindGameObjectWithTag("Journal");

        // Player vectors:
        Vector3 playerPos = player.transform.position;
        Vector3 playerRot = player.transform.rotation.eulerAngles;

        // Interactable object save:
        List<InteractableObjectContainer> IntObjConList = new List<InteractableObjectContainer>();

        string tempSaveString = "";

        //// Save interactables: ////
        for (int i = 0; i < interactables.Length; i++)
        {
            InteractableObjectContainer tempIntObjCon = new InteractableObjectContainer();
            Interactable tempIntScript = interactables[i].GetComponent<Interactable>();

            tempIntObjCon.position = interactables[i].transform.position;
            tempIntObjCon.rotation = interactables[i].transform.rotation.eulerAngles;

            tempIntObjCon.hasItem = tempIntScript.hasItem;
            tempIntObjCon.hasClue = tempIntScript.hasClue;
            tempIntObjCon.hasNote = tempIntScript.hasNote;
            tempIntObjCon.hasKeyItem = tempIntScript.hasKeyItem;

            IntObjConList.Add(tempIntObjCon);
            tempSaveString = tempSaveString + JsonUtility.ToJson(IntObjConList[i]);
        }
        File.WriteAllText(saveLocation + "interactables.txt", tempSaveString);

        //// Save player position and rotation: ////
        tempSaveString = "";

        tempSaveString = JsonUtility.ToJson(playerPos) + JsonUtility.ToJson(playerRot);
        File.WriteAllText(saveLocation + "player.txt", tempSaveString);

        //// Save journal data: ////
        tempSaveString = "";

        if (journal.GetComponent<UI_Journal>() != null)
        {
            tempSaveString = JsonUtility.ToJson(journal.GetComponent<UI_Journal>().clueTexts);
            File.WriteAllText(saveLocation + "activeClues.txt", tempSaveString);
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

public class JournalContainer
{
    public List<string> activeClues;
    public List<string> activeNotes;
}
