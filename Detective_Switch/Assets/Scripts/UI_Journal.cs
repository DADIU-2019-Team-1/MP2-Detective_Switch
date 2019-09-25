using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Journal : MonoBehaviour
{

    List<string> clueTexts;
    public string noteText = "You have no notes yet..";
    bool noNotes = true;

    public string defaultClueText = "You don't have any clues at the moment";
    private string defaultCount = "0/0";
    private int currentIndex = 0;

    // UI Elements:
    public Text clueTextObj;
    public Text clueCountObj;
    public Text noteTextObj;

    private void Awake()
    {
        clueTexts = new List<string>();
        noteTextObj.text = noteText;

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
            clueTextObj.text = clueTexts[currentIndex];
            clueCountObj.text = (currentIndex + 1) + "/" + clueTexts.Count;
        }

    }

    public void NextButtonPress()
    {
        if (clueTexts.Count != 0)
            currentIndex = (currentIndex + 1) % clueTexts.Count;
    }

    public void PrevButtonPress()
    {
        if (clueTexts.Count != 0)
        {
            if (currentIndex == 0)
            {
                currentIndex = clueTexts.Count - 1;
            }
            else
            {
                currentIndex = (currentIndex - 1);
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
        if (noNotes)
        {
            noteText = note;
            noteTextObj.text = noteText;
            noNotes = false;
        }
        else
        {
            noteText = noteText + "\n" + note;
            noteTextObj.text = noteText;
        }
    }
}
