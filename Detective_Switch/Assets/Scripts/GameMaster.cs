using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    private float moveSpeed;
    // Start is called before the first frame update
    public void Awake()
    {
        CreateGameMaster();
    }


    // Update is called once per frame
    void Update()
    {
        
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

    public void SetMoveSpeed(float _moveSpeed) {
        moveSpeed = _moveSpeed;
    }
}
