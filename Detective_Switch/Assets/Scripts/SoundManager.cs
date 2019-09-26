using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public GameObject player;

    private Material walkMaterial;
    private bool isInMenu;

    private float runtime; //done

    private float distanceToWindow; //done
    private GameObject[] windowObjects;
    public float maxDistanceToWindows = 5.0f;

    private float roomSize;
    private int level;   

    void Awake() 
    {
        CreateSoundManager();
    }

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

    void Start()
    {
        windowObjects = GameObject.FindGameObjectsWithTag("WindowSound");
    }

    void Update()
    {
        runtime = Time.time;
        distanceToWindow = calculateDistanceToWindows();
        Debug.Log(distanceToWindow);
    }

    private float calculateDistanceToWindows()
    {
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < windowObjects.Length; i++)
        {
            float dist = Vector3.Distance(player.transform.position, windowObjects[i].transform.position);
            shortestDistance = dist < shortestDistance ? dist : shortestDistance;
        }

        Debug.Log("------------");

        Debug.Log(shortestDistance);

        shortestDistance = 100.0f - (100.0f / maxDistanceToWindows * shortestDistance);
        shortestDistance = Mathf.Clamp(shortestDistance, 0, 100);

        return shortestDistance;
    }

    /*
    material beneith
    marble, carbet, wood

    pri 2
    number - how many keyitems has been picked up 1-10 - 1 when game begins

    pri 1
    is in menu boolean

    0 - 100: how long has the game been running - 100 is 5 minutes (for now)

    distance to nearest window
    0 - 100: 100 is VERY near - 0 is VERY far

    Menu:
    Music slider 0 - 100
    SFX slider 0 - 100

    (nice to have)
    Room size
    0 - 100: 100 is the size of the largest room

    collider on feet of player
    call event

    sound on menu buttons
    one single sound
    */
}
