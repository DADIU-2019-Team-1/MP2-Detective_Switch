using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UI_Journal : MonoBehaviour
{
    // Text content of the journal:
    List<string> clueTexts, noteTexts;
    List<string> notesEN, cluesEN, notesDA, cluesDA;
    string caseEN = "", caseDA = "";

    public bool readCSV = true;
    [Tooltip("Must contain the full path, filename and filetype!")]
    public string filePathCSV = "Assets/Resources/CSV/JournalData.csv";
    public string defaultNoteText = "You have no notes.";
    public string defaultClueText = "You don't have any clues at the moment.";
    private string defaultCount = "0/0";
    private int currentClueIndex = 0;
    private int currentNoteIndex = 0;
    private bool isEnglish = true;

    // UI Elements:
    public Text clueTextObj, clueCountObj, noteTextObj, noteCountObj, caseTextObj;
    public Text caseBtnTextObj, clueBtnTextObj, noteBtnTextObj;

    private void Awake()
    {
        clueTexts = new List<string>();
        noteTexts = new List<string>();
        notesDA = new List<string>();
        cluesDA = new List<string>();
        notesEN = new List<string>();
        cluesEN = new List<string>();

        if (readCSV)
            ReadJournalCSV();
    }

    private void Start()
    {
        if (FindObjectOfType<GameMaster>() != null)
        {
            FindObjectOfType<GameMaster>().localizationEvent += ChangeLanguage;
        }
        else
        {
            Debug.LogError("Journal Error: GameMaster not found!");
        }

        if (caseTextObj != null)
            caseTextObj.text = caseEN;
    }

    void Update()
    {
        if (clueTexts.Count == 0)
        {
            clueTextObj.text = defaultClueText;
            clueCountObj.text = defaultCount;
        }
        else
        {
            clueTextObj.text = clueTexts[currentClueIndex];
            clueCountObj.text = (currentClueIndex + 1) + "/" + clueTexts.Count;
        }

        if (noteTexts.Count == 0)
        {
            noteTextObj.text = defaultNoteText;
            noteCountObj.text = defaultCount;
        }
        else
        {
            noteTextObj.text = noteTexts[currentNoteIndex];
            noteCountObj.text = (currentNoteIndex + 1) + "/" + noteTexts.Count;
        }
    }

    public void NextButtonPress(bool isClue)
    {
        if (isClue)
        {
            if (clueTexts.Count != 0)
                currentClueIndex = (currentClueIndex + 1) % clueTexts.Count;
        }
        else
        {
            if(noteTexts.Count != 0)
                currentNoteIndex = (currentNoteIndex + 1) % noteTexts.Count;
        }

    }

    public void PrevButtonPress(bool isClue)
    {
        if (isClue)
        {
            if (clueTexts.Count != 0)
            {
                if (currentClueIndex == 0)
                {
                    currentClueIndex = clueTexts.Count - 1;
                }
                else
                {
                    currentClueIndex = (currentClueIndex - 1);
                }
            }
        }
        else
        {
            if (noteTexts.Count != 0)
            {
                if (currentNoteIndex == 0)
                {
                    currentNoteIndex = noteTexts.Count - 1;
                }
                else
                {
                    currentNoteIndex = (currentNoteIndex - 1);
                }
            }
        }
    }

    public void AddClueToJournal(string clue)
    {
        clueTexts.Add(clue);
    }

    public void RemoveClueFromJournal(int index)
    {
        clueTexts.RemoveAt(index);
    }

    public void RemoveClueFromJournal(string specificText)
    {
        clueTexts.Remove(specificText);
    }

    public void AddNoteToJournal(string note)
    {
        noteTexts.Add(note);
    }

    private void ChangeLanguage()
    {
        isEnglish = !isEnglish;

        if (isEnglish)
        {
            caseTextObj.text = caseEN;
            caseBtnTextObj.text = "Case"; clueBtnTextObj.text = "Clues"; noteBtnTextObj.text = "Notes";
        }
        else
        {
            caseTextObj.text = caseDA;
            caseBtnTextObj.text = "Sag"; clueBtnTextObj.text = "Spor"; noteBtnTextObj.text = "Noter";
        }
    }

    public string GetClue(int index)
    {
        if (index > cluesEN.Count || index < 0)
            return null;

        if (isEnglish)
        {
            return cluesEN[index];
        }
        else
        {
            return cluesDA[index];
        }
    }

    public string GetNote(int index)
    {
        if (index > notesEN.Count || index < 0)
            return null;

        if (isEnglish)
        {
            return notesEN[index];
        }
        else
        {
            return notesDA[index];
        }
    }

    public void ReadJournalCSV()
    {
        if (filePathCSV == null)
        {
            Debug.LogError("Journal Error: CSV path not set correctly!");
            return;
        }

        StreamReader strReader = new StreamReader(filePathCSV);

        bool endOfFile = false;
        bool firstRun = true;
        bool secondRun = true;

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
                
                if(secondRun) 
                {
                    caseEN = tempDataValues[4];
                    caseDA = tempDataValues[5];
                    secondRun = false;
                }

                cluesEN.Add(tempDataValues[0]);
                cluesDA.Add(tempDataValues[1]);
                notesEN.Add(tempDataValues[2]);
                notesDA.Add(tempDataValues[3]);              
            }
        }   
    }
}