using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalToggler : MonoBehaviour
{
    public bool journalState = false;
    public GameObject journalPanelObj;

    public void toggleJournal()
    {
        journalState = !journalState;
        UpdateJournal();
    }

    public void SetJournalState(bool state)
    {
        journalState = state;
        UpdateJournal();
    }

    private void UpdateJournal()
    {
        GameMaster.instance.SetJournalIsOpen(journalState);
        journalPanelObj.SetActive(journalState);
    }
}
