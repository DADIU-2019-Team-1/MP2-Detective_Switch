using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public GameObject Self;
    public float totalTime = 1f; //2 minutes
    public bool rise = true;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        totalTime -= Time.deltaTime;
        UpdateLevelTimer(totalTime);

        if(rise)
        {
            transform.position += Vector3.up *0.005f;
        }
        
    }

    public void UpdateLevelTimer(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.RoundToInt(totalSeconds % 60f);

        string formatedSeconds = seconds.ToString();

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }
        Debug.Log(minutes.ToString("00") + ":" + seconds.ToString("00"));
        if (seconds < 0)
        {
            Self.SetActive(false);
            transform.position = startPos;
        }

    }
}