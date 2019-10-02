using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFX_Slider : MonoBehaviour
{
    public AK.Wwise.RTPC Menuslider_SFX;
    public float sfxSliderValue, musicSliderValue, sliderValue;
    void Start()
    {
        // SetRTPCValue(wwise name, float from slider)
        // Load float from slider, and place it in the float slot of SetRTPCValue.
        AKRESULT effectSound = AkSoundEngine.SetRTPCValue("SFX_Slider", GameMaster.instance.GetMusicLevel());
        AKRESULT musicSound = AkSoundEngine.SetRTPCValue("Music_Slider", GameMaster.instance.GetSFXLevel());

        if (gameObject.name == "SFX_Slider")
        {
            Debug.Log("Setting SFX slider to " + GameMaster.instance.GetSFXLevel());
            GetComponent<Slider>().value = GameMaster.instance.GetSFXLevel();
        }

        if (gameObject.name == "Music_Slider")
        {
            Debug.Log("Setting music slider to " + GameMaster.instance.GetMusicLevel());
            GetComponent<Slider>().value = GameMaster.instance.GetMusicLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        sliderValue = GetComponent<Slider>().value;

        if (gameObject.name == "SFX_Slider")
        {
            GameMaster.instance.SetSFXLevel((int)sliderValue);
            AkSoundEngine.SetRTPCValue("SFX_Slider", sliderValue);
        }
        
        else if(gameObject.name == "Music_Slider")
        {
            GameMaster.instance.SetMusicLevel((int)sliderValue);
            AkSoundEngine.SetRTPCValue("Music_Slider", sliderValue);
        }


    }
}

