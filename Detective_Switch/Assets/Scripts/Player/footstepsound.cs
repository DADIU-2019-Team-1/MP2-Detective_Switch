using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepsound : MonoBehaviour
{
    // Start is called before the first frame update
    public AK.Wwise.Event footStepPlay;
   

    public Material currentMatFootStep;

    public string lastMaterial;

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        footStepPlay.Post(gameObject);
    }
}
