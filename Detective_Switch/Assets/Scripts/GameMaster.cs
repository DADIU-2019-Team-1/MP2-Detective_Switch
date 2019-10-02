using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    /// --- Public

    /// --- Private
    private float moveSpeed;

    private int musicLevel = 70;
    private int sfxLevel = 70;
    private bool localization = false; // "False == English, True == Danish"

    /// --- Events
    public delegate void LocalizationDelegate();
    public event LocalizationDelegate localizationEvent;

    private bool playerCanMove = true;
    private bool playerIsInMenu = true;
    private bool journalIsOpen = false;

    public void Awake()
    {
        CreateGameMaster();
    }

    void Update()
    {

    }

    public void SetJournalIsOpen(bool state)
    {
        journalIsOpen = state;
    }

    public bool GetJournalIsOpen()
    {
        return journalIsOpen;
    }

    public void SetMenuIsOpen(bool state)
    {
        playerIsInMenu = state;
        SoundManager.instance.SetMenuIsOpen(playerIsInMenu);
    }

    public bool GetMenuIsOpen()
    {
        return playerIsInMenu;
    }

    public bool GetPlayerCanMove()
    {
        return playerCanMove;
    }

    public void SetPlayerCanMove(bool canMove)
    {
        playerCanMove = canMove;
    }

    public void SetLocalization()
    {
        if (localizationEvent != null)
        {
            Debug.Log("Localization event triggered!");
            localizationEvent();
        }
            localization = !localization;
    }

    void CreateGameMaster()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetMoveSpeed(float _moveSpeed)
    {
        moveSpeed = _moveSpeed;
    }

    public GameObject FindObjectFromParentName(string parent, string targetName)
    {
        GameObject obj = null;
        Transform[] objChildren = GameObject.Find(parent).GetComponentsInChildren<Transform>(true);
        foreach (Transform child in objChildren)
        {
            if (child.gameObject.name == targetName)
            {
                obj = child.gameObject;
            }
        }
        return obj;
    }

    public GameObject FindObjectFromParentObject(GameObject parent, string targetName)
    {
        GameObject obj = null;
        Transform[] objChildren = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in objChildren)
        {
            if (child.gameObject.name == targetName)
            {
                obj = child.gameObject;
            }
        }
        return obj;
    }

    public int GetMusicLevel()
    {
        return musicLevel;
    }

    public void SetMusicLevel(int music_level)
    {
        musicLevel = music_level;
    }

    public int GetSFXLevel()
    {
        return sfxLevel;
    }

    public void SetSFXLevel(int sfx_level)
    {
        sfxLevel = sfx_level;
    }
}
