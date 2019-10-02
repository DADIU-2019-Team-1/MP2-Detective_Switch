using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AK.Wwise.Event footStepPlay;
    public AK.Wwise.Event carpetPlay;
    public AK.Wwise.Event marblePlay;

    public AK.Wwise.Event woodPlay;

    public Material currentMatFootStep;

    public string lastMaterial;

    // Start is called before the first frame update
    void Start()
    {
        carpetPlay.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        footStepPlay.Post(gameObject);
    }
}
