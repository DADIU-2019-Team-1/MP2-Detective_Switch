﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    // Main menu:
    public Button continueBtn;
    public Text continueBtnText;
    public Text newGameBtnText;
    public Text optionsBtnText;

    // Options menu:
    public Text optionsBackBtnText;
    public Text musicSliderText;
    public Text languageText;
    public Text tutorialText;

    private bool isEnglish = true;

    void Start()
    {
        continueBtn.interactable = false;

        if (FindObjectOfType<GameMaster>() != null)
        {
            FindObjectOfType<GameMaster>().localizationEvent += LanguageChange;
        }
    
        if (PlayerPrefs.GetInt("previousGame") == 1)
        {
            continueBtn.interactable = false;
        }
    }

    private void LanguageChange()
    {
        isEnglish = !isEnglish;

        if (isEnglish)
        {
            continueBtnText.text = "continue";
            newGameBtnText.text = "new game";
            optionsBtnText.text = "options";
            optionsBackBtnText.text = "Back";
            musicSliderText.text = "music";
            languageText.text = "language";
            tutorialText.text = "tap to change the flashlight mode";
}
        else
        {
            continueBtnText.text = "fortsæt";
            newGameBtnText.text = "nyt spil";
            optionsBtnText.text = "indstillinger";
            optionsBackBtnText.text = "Tilbage";
            musicSliderText.text = "musik";
            languageText.text = "sprog";
            tutorialText.text = "tryk for at skifte lommelygte tilstand";
        }
    }


}
