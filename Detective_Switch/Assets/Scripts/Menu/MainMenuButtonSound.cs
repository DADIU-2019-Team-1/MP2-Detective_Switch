using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonSound : MonoBehaviour
{
    public string ButtonSound;

    public void PlayButtonSound()
    {
        AkSoundEngine.PostEvent(ButtonSound, gameObject);
    }
}
