using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_Menu_Button_SFX : MonoBehaviour
{
    public void Case_File_SFX()
    {
        AkSoundEngine.PostEvent("Play_Case_File_Button", gameObject);
    }

    public void Main_Menu_Button_SFX()
    {
        AkSoundEngine.PostEvent("Play_Menu_Button", gameObject);
    }

    public void Main_Inventory_Button_SFX()
    {
        AkSoundEngine.PostEvent("Play_Inventory_Button", gameObject);
    }

    public void Blacklight_ON()
    {
        AkSoundEngine.PostEvent("Play_Blacklight_On", gameObject);
    }

    public void Blacklight_OFF()
    {
        AkSoundEngine.PostEvent("Play_Blacklight_OFF", gameObject);
    }


    
}
