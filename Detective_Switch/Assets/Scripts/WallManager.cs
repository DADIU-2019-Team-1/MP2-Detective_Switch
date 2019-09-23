using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class WallManager : MonoBehaviour
{
    // Controllers
    // Controllers for the modular wall setup
    [Range(1, 3)]
    public int WallType = 1;

    [Range(1, 3)]
    public int WallColour = 1;

    [Range(1, 3)]
    public int PanelStyle = 1;

    [Range(1, 3)]
    public int DoorWindowStyle = 1;

    // Mesh components
    // The three different base wall cutouts
    [HideInInspector] public Mesh WallStandard;
    [HideInInspector] public Mesh WallWindow;
    [HideInInspector] public Mesh WallDoor;

    // Three different panelling styles (no panel, thick panel, small panel)
    [HideInInspector] public Mesh PanelStyle01;
    [HideInInspector] public Mesh PanelStyle02;
    [HideInInspector] public Mesh PanelStyle03;
    [HideInInspector] public Mesh PanelStyle01D;
    [HideInInspector] public Mesh PanelStyle02D;
    [HideInInspector] public Mesh PanelStyle03D;

    // Three different windows (Follows panelling styles i guess)
    [HideInInspector] public Mesh WindowStyle01;
    [HideInInspector] public Mesh WindowStyle02;
    [HideInInspector] public Mesh WindowStyle03;

    // Three different windows (Follows panelling styles i guess)
    [HideInInspector] public Mesh DoorStyle01;
    [HideInInspector] public Mesh DoorStyle02;
    [HideInInspector] public Mesh DoorStyle03;


    // List of usable mats
    [HideInInspector] public Material Mat1;
    [HideInInspector] public Material Mat2;
    [HideInInspector] public Material Mat3;

    // Actors
    // Child actors
    [HideInInspector] public GameObject window;
    [HideInInspector] public GameObject decor;
    [HideInInspector] public GameObject panels;


    // Start is called before the first frame update
    void Start()
    {
    }
    void Update()
    {
        // Logic that sorts out wall type
        if (Application.isEditor && !Application.isPlaying)
        {
            if (WallType == 1)
            {
                GetComponent<MeshFilter>().sharedMesh = WallStandard;
                window.SetActive(false);
                decor.SetActive(false);
            }

            if (WallType == 2)
            {
                GetComponent<MeshFilter>().sharedMesh = WallWindow;
                window.SetActive(true);
                decor.SetActive(false);

            }

            if (WallType == 3)
            {
                GetComponent<MeshFilter>().sharedMesh = WallDoor;
                window.SetActive(false);
                decor.SetActive(true);
            }

            if (WallColour == 1)
            {
                GetComponent<MeshRenderer>().material = Mat1;

            }

            if (WallColour == 2)
            {
                GetComponent<MeshRenderer>().material = Mat2;


            }

            if (WallColour == 3)
            {
                GetComponent<MeshRenderer>().material = Mat3;

            }

            // Logic that sorts out panel styles
            if (PanelStyle == 1)
            {
                if (WallType == 3)
                {
                    panels.GetComponent<MeshFilter>().sharedMesh = PanelStyle01D;
                }
                else
                {
                    panels.GetComponent<MeshFilter>().sharedMesh = PanelStyle01;
                }
            }

            if (PanelStyle == 2)
            {
                if (WallType == 3)
                {
                    panels.GetComponent<MeshFilter>().sharedMesh = PanelStyle02D;
                }
                else
                {
                    panels.GetComponent<MeshFilter>().sharedMesh = PanelStyle02;
                }
            }

            if (PanelStyle == 3)
            {
                if (WallType == 3)
                {
                    panels.GetComponent<MeshFilter>().sharedMesh = PanelStyle03D;
                }
                else
                {
                    panels.GetComponent<MeshFilter>().sharedMesh = PanelStyle03;
                }
            }

            // Logic that sorts out window styles
            if (DoorWindowStyle == 1)
            {
                if (WallType == 3)
                {
                    decor.GetComponent<MeshFilter>().sharedMesh = DoorStyle01;
                }
                else
                {
                    window.GetComponent<MeshFilter>().sharedMesh = WindowStyle01;
                }
            }

            if (DoorWindowStyle == 2)
            {
                if (WallType == 3)
                {
                    decor.GetComponent<MeshFilter>().sharedMesh = DoorStyle02;
                }
                else
                {
                    window.GetComponent<MeshFilter>().sharedMesh = WindowStyle02;
                }
            }

            if (DoorWindowStyle == 3)
            {
                if (WallType == 3)
                {
                    decor.GetComponent<MeshFilter>().sharedMesh = DoorStyle03;
                }
                else
                {
                    window.GetComponent<MeshFilter>().sharedMesh = WindowStyle03;
                }
            }
        }
    }
}