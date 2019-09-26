using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private float roomSize;
    private float distanceToNearestWindow;
    private Material footMat;

    void Awake() 
    {
        CreateSoundManager();
    }
    // Start is called before the first frame update

    void CreateSoundManager()
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

    public void SetRoomSize(float _roomSize) {
        roomSize = _roomSize;
    }

    public void SetDistanceToWindow(float _distanceToNearestWindow) {
        distanceToNearestWindow = _distanceToNearestWindow;
    }

    public void SetFootMat(Material _footMat) {
        footMat = _footMat;
    }
}
