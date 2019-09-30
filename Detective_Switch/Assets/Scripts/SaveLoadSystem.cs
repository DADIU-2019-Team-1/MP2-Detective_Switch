using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class SaveLoadSystem : MonoBehaviour
{
    private const string SAVE_SEPERATOR = "#SAVE-VALUE#";
    public bool newGame = true;
    public string saveLocation = "Assets/Resources/SaveFiles/";

    private void Awake()
    {
        if (newGame)
        {
            PlayerPrefs.SetInt("previousGame", 1);

        }
        else if (PlayerPrefs.GetInt("previousGame") == 1)
        {
            // LoadGame(); YYY
        }
    }

    private void Start()
    {
        SaveGame();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void LoadGame()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null || GameObject.FindGameObjectsWithTag("interactable") == null || GameObject.FindGameObjectWithTag("Journal") == null)
            return;

        string tempLoadString = "";

        if (File.Exists(saveLocation + "interactables.txt") && File.Exists(saveLocation + "player.txt") && File.Exists(saveLocation + "activeJournal.txt"))
        {
            tempLoadString = File.ReadAllText(saveLocation + "interactables.txt");

            //// Load interactables: ////
            List<InteractableObjectContainer> IntObjConList = new List<InteractableObjectContainer>();
            GameObject[] interactables = GameObject.FindGameObjectsWithTag("interactable");
            string[] tempDataString = tempLoadString.Split(new[] {SAVE_SEPERATOR}, System.StringSplitOptions.None);

            for (int i = 1; i < tempDataString.Length; i++) // Start from 1
            {
                IntObjConList.Add(JsonUtility.FromJson<InteractableObjectContainer>(tempDataString[i]));               
            }
            // Debug.Log(IntObjConList[1].position + " " + IntObjConList[1].rotation + " " + IntObjConList[1].hasClue);

            for (int i = 0; i < interactables.Length; i++)
            {
                Interactable tempIntScript = interactables[i].GetComponent<Interactable>();
                for (int j = 0; j < IntObjConList.Count; j++)
                {
                    if (tempIntScript.iD == IntObjConList[j].uniqueID)
                    {
                        interactables[i].transform.position = IntObjConList[j].position;
                        interactables[i].transform.rotation = Quaternion.Euler(IntObjConList[j].rotation);
                        tempIntScript.hasClue = IntObjConList[j].hasClue;
                        tempIntScript.hasNote = IntObjConList[j].hasNote;
                        tempIntScript.hasKeyItem = IntObjConList[j].hasKeyItem;
                        tempIntScript.hasItem = IntObjConList[j].hasItem;
                    }
                }               
            }

            //// Load player pos and rot: ////
            tempLoadString = File.ReadAllText(saveLocation + "player.txt");
            tempDataString = tempLoadString.Split(new[] { SAVE_SEPERATOR }, System.StringSplitOptions.None);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = JsonUtility.FromJson<Vector3>(tempDataString[0]);
            player.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(tempDataString[1]));

            //// Load journal data: ////
            tempLoadString = File.ReadAllText(saveLocation + "activeJournal.txt");
            JournalContainer journalCon = new JournalContainer();
            journalCon = JsonUtility.FromJson<JournalContainer>(tempLoadString);
            if (GameObject.FindGameObjectWithTag("Journal") != null)
            {
                GameObject journal = GameObject.FindGameObjectWithTag("Journal");
                if (journal.GetComponent<UI_Journal>() != null)
                {
                    UI_Journal journalScript = journal.GetComponent<UI_Journal>();
                    journalScript.clueTexts = journalCon.activeClues;
                    journalScript.noteTexts = journalCon.activeNotes;
                }
                else
                {
                    Debug.LogError("Load error: journal not found, or journal tag not on correct object!");
                }
            }
            else
            {
                Debug.LogError("Load error: journal not found, or journal tag not on correct object!");
            }
        }    
    }

    public bool GetNewGameBool()
    {
        return newGame;
    }

    public void SetNewGameBool(bool setTo)
    {
        newGame = setTo;
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

            tempIntObjCon.uniqueID = tempIntScript.iD;
            tempIntObjCon.position = interactables[i].transform.position;
            tempIntObjCon.rotation = interactables[i].transform.rotation.eulerAngles;

            tempIntObjCon.hasItem = tempIntScript.hasItem;
            tempIntObjCon.hasClue = tempIntScript.hasClue;
            tempIntObjCon.hasNote = tempIntScript.hasNote;
            tempIntObjCon.hasKeyItem = tempIntScript.hasKeyItem;

            IntObjConList.Add(tempIntObjCon);
            tempSaveString = tempSaveString + SAVE_SEPERATOR + JsonUtility.ToJson(IntObjConList[i]);
        }
        File.WriteAllText(saveLocation + "interactables.txt", tempSaveString);

        //// Save player position and rotation: ////
        tempSaveString = "";

        tempSaveString = JsonUtility.ToJson(playerPos) + SAVE_SEPERATOR + JsonUtility.ToJson(playerRot);
        File.WriteAllText(saveLocation + "player.txt", tempSaveString);

        //// Save journal data: ////
        tempSaveString = "";

        if (journal.GetComponent<UI_Journal>() != null)
        {
            JournalContainer tempJournal = new JournalContainer();
            tempJournal.activeClues = journal.GetComponent<UI_Journal>().clueTexts;
            tempJournal.activeNotes = journal.GetComponent<UI_Journal>().noteTexts;

            tempSaveString = JsonUtility.ToJson(tempJournal);
            File.WriteAllText(saveLocation + "activeJournal.txt", tempSaveString);
        }  
    }
}

public class InteractableObjectContainer
{
    public int uniqueID;

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
