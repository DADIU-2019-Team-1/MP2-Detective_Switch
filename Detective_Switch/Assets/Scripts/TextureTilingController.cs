using UnityEngine;
using System.Collections;

[ExecuteInEditMode]

// Update is called once per frame
public class TextureTilingController : MonoBehaviour
{
    public GameObject WallStandard;
    Renderer cubeRenderer;
    void Start()
    {
        

    }
    void Update()
    {
        Debug.Log(WallStandard.transform.localScale[0]);
        Debug.Log(GetComponent<Renderer>().sharedMaterial.GetTextureScale("_MainTex"));

        GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(WallStandard.transform.localScale[0], WallStandard.transform.localScale[1]));

    }
}

