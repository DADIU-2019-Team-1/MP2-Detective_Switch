using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // should be found by tag in start
    public GameObject player;

    private Material walkMaterial;

    // in menu
    public AK.Wwise.State MenuOpen;
    public AK.Wwise.State MenuClose;
    public bool menuIsOpen;
    private bool wwiseMenuIsOpen;

    // time of day
    public AK.Wwise.RTPC timeOfDay;
    public int dayLength = 300;

    // distance to window
    public AK.Wwise.RTPC distanceToWindow;
    public float maxDistanceToWindows = 5.0f;

    private GameObject[] windowObjects;

    // roomsize
    private float roomSize;

    // progression
    private int progression;

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
        wwiseMenuIsOpen = !menuIsOpen;
    }

    void Update()
    {
        timeOfDay.SetGlobalValue(calcTimeOfDay());
        distanceToWindow.SetGlobalValue(calcDistanceToWindows());
        setMenuState();
    }

    private void setMenuState()
    {
        if (menuIsOpen == wwiseMenuIsOpen)
            return;

        wwiseMenuIsOpen = menuIsOpen;
        if (menuIsOpen)
        {
            MenuOpen.SetValue();
            Debug.Log("Menu Open Sound");
        } else
        {
            MenuClose.SetValue();
            Debug.Log("Menu Close Sound");
        }
    }

    private float calcTimeOfDay()
    {
        float time = 100.0f / dayLength * Time.time;
        return Mathf.Clamp(time, 0, 100);
    }

    private float calcDistanceToWindows()
    {
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < windowObjects.Length; i++)
        {
            float dist = Vector3.Distance(player.transform.position, windowObjects[i].transform.position);
            shortestDistance = dist < shortestDistance ? dist : shortestDistance;
        }

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
