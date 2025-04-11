using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization.Scripts;

namespace Assets.SimpleLocalization
{
	public class SetLocalizeText : MonoBehaviour
    {
        public Button frenchButton, englishButton;
        public Sprite frenchButtonOn, frenchButtonOff, englishButtonOn, englishButtonOff;

        private void Start()
        {
            string savedLanguage = PlayerPrefs.GetString("Localization", "English");
            SetLocalization(savedLanguage, false);
        }

        public void SetLocalization(string localization)
		{
            SetLocalization(localization, true);
        }

        private void SetLocalization(string localization, bool save)
        {
            LocalizationManager.Language = localization;
            LocalizationManager._language = localization;
            
            if (save) PlayerPrefs.SetString("Localization", localization);
            
            UpdateButtonSprites(localization);
        }

        private void UpdateButtonSprites(string localization)
        {
            bool isFrench = localization == "French";
            
            frenchButton.image.sprite = isFrench ? frenchButtonOn : frenchButtonOff;
            englishButton.image.sprite = isFrench ? englishButtonOff : englishButtonOn;
        }
	}
}