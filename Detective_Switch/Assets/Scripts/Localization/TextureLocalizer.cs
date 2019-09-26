using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureLocalizer : MonoBehaviour
{
    [Header("Place on an object with a text texture", order = 1)]
    public Texture englishTextTexture;
    public Texture danishTextTexture;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (englishTextTexture != null)
        {
            rend.material.mainTexture = englishTextTexture;
            rend.material.SetTexture("_BaseMap", englishTextTexture);
        }
        else
            Debug.Log("The object \"" + gameObject.name + "\" is missing an ENGLISH texture in it's TextureLocalizer!");

        // Subscribe to the localization event
        FindObjectOfType<GameMaster>().localizationEvent += OnLocalizationChange;
    }

    public void OnLocalizationChange()
    {
        Debug.Log("Detected localization event for Gameobject: " + gameObject.name);

        if (rend.material.GetTexture("_BaseMap") == englishTextTexture)
        {
            if (danishTextTexture != null)
                rend.material.SetTexture("_BaseMap", danishTextTexture);
            else
                Debug.Log("The object \"" + gameObject.name + "\" is missing a DANISH texture in it's TextureLocalizer!");
        }
        else
            rend.material.SetTexture("_BaseMap", englishTextTexture);
    }
}
