﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    /// --- Public

    /// --- Private
    private float moveSpeed;
    private bool localization = false; // "False == English, True == Danish"

    /// --- Events
    public delegate void LocalizationDelegate();
    public event LocalizationDelegate localizationEvent;

    public void Awake()
    {
        CreateGameMaster();
    }


    // Update is called once per frame
    void Update()
    {

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
}
