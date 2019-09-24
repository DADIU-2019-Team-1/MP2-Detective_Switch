using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureLocalizer : MonoBehaviour
{
    [Header("Place on an object with a text texture", order = 1)]
    public Texture englishTextTexture;
    public Texture danishTextTexture;
    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetTexture("Hello World", englishTextTexture);
        // Subscribe to the localization event
        FindObjectOfType<GameMaster>().localizationEvent += OnLocalizationChange;
    }

    public void OnLocalizationChange()
    {
        Debug.Log("Detected localization event for Gameobject: " + gameObject.name);
        // Unsubscribe to the localization event
        FindObjectOfType<GameMaster>().localizationEvent -= OnLocalizationChange;

        if (mat.mainTexture == englishTextTexture)
            mat.mainTexture = danishTextTexture;
        else
            mat.mainTexture = englishTextTexture;
    }
}
