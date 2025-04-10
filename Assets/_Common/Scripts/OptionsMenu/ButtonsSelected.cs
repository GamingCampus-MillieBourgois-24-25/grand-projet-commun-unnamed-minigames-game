using UnityEngine;
using UnityEngine.UI;

public class ButtonsSelected : MonoBehaviour
{
    public Button buttonsFrenchLanguage;
    public Button buttonsEnglishLanguage;

    public void OnSelectedLanguageChanged()
    {
        if (buttonsFrenchLanguage.interactable)
        {
            buttonsFrenchLanguage.interactable = false;
            buttonsEnglishLanguage.interactable = true;
        }
        if (buttonsEnglishLanguage.interactable)
        {
            buttonsFrenchLanguage.interactable = true;
            buttonsEnglishLanguage.interactable = false;
        }
    }
}
