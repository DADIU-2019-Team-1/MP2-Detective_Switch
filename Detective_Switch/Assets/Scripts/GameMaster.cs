using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    private float moveSpeed;

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
        if (Input.GetKeyDown("b"))
            if (localizationEvent != null)
            {
                Debug.Log("Localization event triggered!");
                localizationEvent();
            }
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
