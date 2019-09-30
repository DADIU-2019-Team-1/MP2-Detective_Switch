using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class WallManager : MonoBehaviour
{
    // Controllers
    // Controllers for the modular wall setup
    [Range(1, 2)]
    public int WallType = 1;

    [Range(1, 3)]
    public int WallVariants = 1;

    [HideInInspector] public Color WallColour = new Color(0.5f, 0.5f, 0.5f);
    [HideInInspector] public Color PanelColour = new Color(0.6f, 0.6f, 0.6f);



    public bool TurnIn = false;
    public bool TurnOut = false;

    public bool HalfLength = false;

    // Mesh components
    // The three different base wall cutouts
    [HideInInspector] public Mesh WallStandard;
    [HideInInspector] public Mesh WallStandardDoor;

    [HideInInspector] public Mesh WallStandardDouble;
    [HideInInspector] public Mesh WallStandardHalf;
    [HideInInspector] public Mesh WallOuterHalf;

    [HideInInspector] public Mesh WallOuter;
    [HideInInspector] public Mesh WallOuterPillar;
    [HideInInspector] public Mesh WallOuterWindow;

    // Three different panelling styles (no panel, thick panel, small panel)
    [HideInInspector] public Mesh WallStandard90In;
    [HideInInspector] public Mesh WallOuter90In;
    [HideInInspector] public Mesh WallStandard90Out;
    [HideInInspector] public Mesh WallOuter90Out;


    // List of usable mats
    public Material Mat2;
    public Material Mat3;


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
                if (WallVariants == 1)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallStandard;
                }
                if (WallVariants == 2)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallStandardDoor;
                }
                if (WallVariants == 3)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallStandardDouble;
                }

            }

            if (WallType == 2)
            {
                if (WallVariants == 1)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallOuter;
                }
                if (WallVariants == 2)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallOuterPillar;
                }
                if (WallVariants == 3)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallOuterWindow;
                }

            }

            if (TurnIn == true)
            {

                if (WallType == 1)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallStandard90In;
                }
                if (WallType == 2)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallOuter90In;
                }
            }
            if (TurnOut == true)
            {

                if (WallType == 1)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallStandard90Out;
                }
                if (WallType == 2)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallOuter90Out;
                }
            }
            var cubeRenderer = GetComponent<Renderer>();
            if (HalfLength == true)
            {

                if (WallType == 1)
                {

                    GetComponent<MeshFilter>().sharedMesh = WallStandardHalf;
                    
                }
                if (WallType == 2)
                {
                    GetComponent<MeshFilter>().sharedMesh = WallOuterHalf;
                }
            }

            //var cubeRenderer = GetComponent<Renderer>();

            //cubeRenderer.sharedMaterial.SetColor("_BaseColor", WallColour);
            //cubeRenderer.sharedMaterials[1].SetColor("_BaseColor", PanelColour);

        }
    }
}