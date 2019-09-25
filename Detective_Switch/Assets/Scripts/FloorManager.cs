using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class FloorManager : MonoBehaviour
{
// Controllers
    // Controllers for the modular wall setup
    [Range(1, 3)]
    public int FloorType = 1;

    [HideInInspector] public Material Mat1;
    [HideInInspector] public Material Mat2;
    [HideInInspector] public Material Mat3;

    void Update()
    {
        // Logic that sorts out wall type
        if (Application.isEditor && !Application.isPlaying)
        {
            if (FloorType == 1)
            {
                GetComponent<MeshRenderer>().material = Mat1;
            }

            if (FloorType == 2)
            {
                GetComponent<MeshRenderer>().material = Mat2;

            }

            if (FloorType == 3)
            {
                GetComponent<MeshRenderer>().material = Mat3;
            }
        }
    }
} 