using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{
    private const string SAVE_SEPERATOR = "#SAVE-VALUE#";
    public bool newGame = true;
    // public string saveLocation = "Assets/Resources/SaveFiles/";

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            SaveGame();
            Debug.Log("Saved Game!");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}

    public void NewGame()
    {
        PlayerPrefs.SetInt("previousGame", 1);

        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");

        if (rooms != null)
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].name == "Dining Hall")
                {
                    rooms[i].SetActive(false);
                }
                else if (rooms[i].name == "BedRoom")
                {
                    rooms[i].SetActive(false);
                }
                else if (rooms[i].name == "Living Room")
                {
                    rooms[i].SetActive(true);
                }
                else if (rooms[i].name == "Hallway")
                {
                    rooms[i].SetActive(true);
                }
                else if (rooms[i].name == "Entrance")
                {
                    rooms[i].SetActive(true);
                }
                else if (rooms[i].name == "Foyer 1st Floor")
                {
                    rooms[i].SetActive(false);
                }

            }
        }
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("previousGame") == 1)
        {
            LoadGame();
        }
        else
        {
            Debug.Log("LoadGame: No previous games to load");
        }
    }

    private void LoadGame()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null || GameObject.FindGameObjectsWithTag("interactable") == null || GameObject.FindGameObjectWithTag("Journal") == null)
            return;

        string tempLoadString = "";

        tempLoadString = PlayerPrefs.GetString("interactableSave"); // File.ReadAllText(saveLocation + "interactables.txt");

        //// Load interactables: ////
        List<InteractableObjectContainer> IntObjConList = new List<InteractableObjectContainer>();
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("interactable");
        string[] tempDataString = tempLoadString.Split(new[] {SAVE_SEPERATOR}, System.StringSplitOptions.None);

        for (int i = 1; i < tempDataString.Length; i++) // Start from 1
        {
            IntObjConList.Add(JsonUtility.FromJson<InteractableObjectContainer>(tempDataString[i]));
        }

        for (int i = 0; i < interactables.Length; i++)
        {
            Interactable tempIntScript = interactables[i].GetComponent<Interactable>();
            for (int j = 0; j < IntObjConList.Count; j++)
            {
                if (tempIntScript.iD == IntObjConList[j].uniqueID)
                {
                    // interactables[i].transform.position = IntObjConList[j].position;
                    interactables[i].transform.rotation = Quaternion.Euler(IntObjConList[j].rotation);
                    tempIntScript.hasClue = IntObjConList[j].hasClue;
                    tempIntScript.hasNote = IntObjConList[j].hasNote;
                    tempIntScript.hasKeyItem = IntObjConList[j].hasKeyItem;
                    tempIntScript.hasItem = IntObjConList[j].hasItem;
                    tempIntScript.toggleState = IntObjConList[j].toggleState;

                    if (IntObjConList[j].hasBeenClicked)
                    {
                        tempIntScript.soundOnInteract = false;
                        tempIntScript.Interact();
                    }
                    tempIntScript.hasBeenClicked = IntObjConList[j].hasBeenClicked;
                }
            }
        }

        //// Load player pos and rot: ////
        tempLoadString = PlayerPrefs.GetString("playerSave");
        tempDataString = tempLoadString.Split(new[] {SAVE_SEPERATOR}, System.StringSplitOptions.None);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = JsonUtility.FromJson<Vector3>(tempDataString[0]);
        player.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(tempDataString[1]));

        //// Load journal data: ////
        tempLoadString = PlayerPrefs.GetString("journalSave");
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

        //// Load key items: ////
        tempLoadString = PlayerPrefs.GetString("keyItemSave");
        List<KeyItemSlotContainer> tempKeyItemSlotContList = new List<KeyItemSlotContainer>();
        GameObject[] keyItemSlots = GameObject.FindGameObjectsWithTag("KeyItemSlot");
        tempDataString = tempLoadString.Split(new[] {SAVE_SEPERATOR}, System.StringSplitOptions.None);

        for (int i = 1; i < tempDataString.Length; i++) // Start from 1
        {
            tempKeyItemSlotContList.Add(JsonUtility.FromJson<KeyItemSlotContainer>(tempDataString[i]));
        }

        for (int i = 0; i < tempKeyItemSlotContList.Count; i++)
        {
            Slot tempSlotScript = keyItemSlots[i].GetComponent<Slot>();

            keyItemSlots[i].GetComponent<Image>().sprite = tempKeyItemSlotContList[i].sourceImage;
            tempSlotScript.item = tempKeyItemSlotContList[i].item;
            tempSlotScript.id = tempKeyItemSlotContList[i].id;
            tempSlotScript.text = tempKeyItemSlotContList[i].text;
            tempSlotScript.empty = tempKeyItemSlotContList[i].empty;
            tempSlotScript.icon = tempKeyItemSlotContList[i].icon;
            tempSlotScript.emptyIcon = tempKeyItemSlotContList[i].emptyIcon;
            tempSlotScript.greyedOutImage = tempKeyItemSlotContList[i].greyedOutImage;
        }

        //// Room state save: ////
        tempLoadString = "";
        RoomContainer tempRoomCon = new RoomContainer();
        tempLoadString = PlayerPrefs.GetString("roomSave");
        tempRoomCon = JsonUtility.FromJson<RoomContainer>(tempLoadString);

        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        
        if (rooms != null)
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].name == "Dining Hall")
                {
                    rooms[i].SetActive(tempRoomCon.diningHall);
                }
                else if (rooms[i].name == "BedRoom")
                {
                    rooms[i].SetActive(tempRoomCon.bedRoom);
                }
                else if (rooms[i].name == "Living Room")
                {
                    rooms[i].SetActive(tempRoomCon.livingRoom);
                }
                else if (rooms[i].name == "Hallway")
                {
                    rooms[i].SetActive(tempRoomCon.hallWay);
                }
                else if (rooms[i].name == "Entrance")
                {
                    rooms[i].SetActive(tempRoomCon.entrance);
                }
                else if (rooms[i].name == "Foyer 1st Floor")
                {
                    rooms[i].SetActive(tempRoomCon.foyer1st);
                }
                else if (rooms[i].name == "Foyer Ground Floor")
                {
                    rooms[i].SetActive(tempRoomCon.foyerGround);
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
        if (GameObject.FindGameObjectWithTag("Player") == null || GameObject.FindGameObjectsWithTag("interactable") == null
            || GameObject.FindGameObjectWithTag("Journal") == null || GameObject.FindGameObjectsWithTag("KeyItemSlot") == null)
        {
            Debug.LogError("SaveGame failed: tags missing or player, interactables, journal or key item slots not found");
            return;
        }

        Debug.Log("Saving Game");
        // Find gameobjects:
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("interactable");
        GameObject journal = GameObject.FindGameObjectWithTag("Journal");
        GameObject[] keyItemSlots = GameObject.FindGameObjectsWithTag("KeyItemSlot");

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
            tempIntObjCon.toggleState = tempIntScript.toggleState;
            tempIntObjCon.hasBeenClicked = tempIntScript.hasBeenClicked;

            IntObjConList.Add(tempIntObjCon);
            tempSaveString = tempSaveString + SAVE_SEPERATOR + JsonUtility.ToJson(IntObjConList[i]);
        }
        PlayerPrefs.SetString("interactableSave", tempSaveString);

        //// Save player position and rotation: ////
        tempSaveString = "";

        tempSaveString = JsonUtility.ToJson(playerPos) + SAVE_SEPERATOR + JsonUtility.ToJson(playerRot);
        PlayerPrefs.SetString("playerSave", tempSaveString);

        //// Save journal data: ////
        tempSaveString = "";

        if (journal.GetComponent<UI_Journal>() != null)
        {
            JournalContainer tempJournal = new JournalContainer();
            tempJournal.activeClues = journal.GetComponent<UI_Journal>().clueTexts;
            tempJournal.activeNotes = journal.GetComponent<UI_Journal>().noteTexts;

            tempSaveString = JsonUtility.ToJson(tempJournal);
            PlayerPrefs.SetString("journalSave", tempSaveString);
        }

        //// Save key items: ////
        tempSaveString = "";
        List<KeyItemSlotContainer> tempKeyItemSlotContList = new List<KeyItemSlotContainer>();

        for (int i = 0; i < keyItemSlots.Length; i++)
        {
            KeyItemSlotContainer tempIntObjCon = new KeyItemSlotContainer();
            Slot tempSlotScript = keyItemSlots[i].GetComponent<Slot>();

            if (tempSlotScript.empty == false)
            {
                tempIntObjCon.sourceImage = keyItemSlots[i].GetComponent<Image>().sprite;
                tempIntObjCon.item = tempSlotScript.item;
                tempIntObjCon.id = tempSlotScript.id;
                tempIntObjCon.text = tempSlotScript.text;
                tempIntObjCon.empty = tempSlotScript.empty;
                tempIntObjCon.icon = tempSlotScript.icon;
                tempIntObjCon.emptyIcon = tempSlotScript.emptyIcon;
                tempIntObjCon.greyedOutImage = tempSlotScript.greyedOutImage;

                tempKeyItemSlotContList.Add(tempIntObjCon);
                tempSaveString = tempSaveString + SAVE_SEPERATOR + JsonUtility.ToJson(tempKeyItemSlotContList[i]);

            }
        }
        PlayerPrefs.SetString("keyItemSave", tempSaveString);

        //// Room state save: ////
        tempSaveString = "";
        RoomContainer tempRoomCon = new RoomContainer();
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");

        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].name == "Dining Hall")
            {
                tempRoomCon.diningHall = rooms[i].activeSelf;
            }
            else if (rooms[i].name == "BedRoom")
            {
                tempRoomCon.bedRoom = rooms[i].activeSelf;
            }
            else if (rooms[i].name == "Living Room")
            {
                tempRoomCon.livingRoom = rooms[i].activeSelf;
            }
            else if (rooms[i].name == "Hallway")
            {
                tempRoomCon.hallWay = rooms[i].activeSelf;
            }
            else if (rooms[i].name == "Entrance")
            {
                tempRoomCon.entrance = rooms[i].activeSelf;
            }
            else if (rooms[i].name == "Foyer 1st Floor")
            {
                tempRoomCon.foyer1st = rooms[i].activeSelf;
            }
            else if (rooms[i].name == "Foyer Ground Floor")
            {
                tempRoomCon.foyerGround = rooms[i].activeSelf;
            }
        }

        tempSaveString = JsonUtility.ToJson(tempRoomCon);
        PlayerPrefs.SetString("roomSave", tempSaveString);
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
    public bool toggleState;
    public bool hasBeenClicked;
}

public class JournalContainer
{
    public List<string> activeClues;
    public List<string> activeNotes;
}

public class KeyItemSlotContainer
{
    public Sprite sourceImage;
    public GameObject item;
    public int id;
    public string text;
    public bool empty;
    public Sprite icon;
    public Sprite emptyIcon;
    public Sprite greyedOutImage;
}

public class RoomContainer
{
    public bool diningHall;
    public bool bedRoom;
    public bool livingRoom;
    public bool hallWay;
    public bool entrance;
    public bool foyer1st;
    public bool foyerGround;
}
