using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteadyCamera : MonoBehaviour
{
    public GameObject target;

    private Vector3 offset;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        offset = new Vector3(-11, 9, -11);
    }

    void Update()
    {
        transform.position = target.transform.position + offset;
    }
}
