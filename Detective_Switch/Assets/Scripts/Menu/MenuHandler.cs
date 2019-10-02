using System.Collections;
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
    public Text tutorialFlashText;
    public Text tutorialSlideText;

    private bool isEnglish = true;

    void Start()
    {
        Color defaultCol = continueBtnText.color;
        continueBtn.interactable = false;
        continueBtnText.color = Color.gray;

        if (FindObjectOfType<GameMaster>() != null)
        {
            FindObjectOfType<GameMaster>().localizationEvent += LanguageChange;
        }
    
        if (PlayerPrefs.GetInt("previousGame") == 1)
        {
            continueBtn.interactable = true;
            continueBtnText.color = defaultCol;
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
            tutorialFlashText.text = "tap to change the flashlight mode";
            tutorialSlideText.text = "hold and drag to move";
}
        else
        {
            continueBtnText.text = "fortsæt";
            newGameBtnText.text = "nyt spil";
            optionsBtnText.text = "indstillinger";
            optionsBackBtnText.text = "Tilbage";
            musicSliderText.text = "musik";
            languageText.text = "sprog";
            tutorialFlashText.text = "tryk for at skifte lommelygte tilstand";
            tutorialSlideText.text = "hold og træk for at gå";
        }
    }


}
