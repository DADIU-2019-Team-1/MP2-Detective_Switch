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
            // Load previous game
        }
    }

    private void Start()
    {
        // SaveGame();
        // LoadGame();
    }

    public void LoadGame()
    {
        string tempLoadString = "";

        if (File.Exists(saveLocation + "interactables.txt"))
        {
            tempLoadString = File.ReadAllText(saveLocation + "interactables.txt");

            List<InteractableObjectContainer> IntObjConList = new List<InteractableObjectContainer>();

            string[] tempDataString = tempLoadString.Split(new[] {SAVE_SEPERATOR}, System.StringSplitOptions.None);

            Debug.Log(tempDataString[0]);
                // JsonUtility.FromJson(tempLoadString, InteractableObjectContainer);
        }
        
    }

    public bool GetNewGameBool()
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
            tempSaveString = tempSaveString + SAVE_SEPERATOR + JsonUtility.ToJson(IntObjConList[i]);
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
