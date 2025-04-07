using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Axoloop.Application.Option
{
    public class LocaleSelector : MonoBehaviour
    {
        private bool _active = false;

        public Button englishButtonLanguage;
        public Button frenchButtonLanguage;
        public Sprite englishOn;
        public Sprite englishOff;
        public Sprite frenchOn;
        public Sprite frenchOff;
        

        private void Start()
        {
            var id = PlayerPrefs.GetInt("LocaleKey", 0);
            ChangeLocale(id);
        }

        public void ChangeLocale(int localeID)
        {
            if (_active == true) return;

            if (localeID == 0)
            {
                englishButtonLanguage.image.sprite = englishOn;
                frenchButtonLanguage.image.sprite = frenchOff;

                englishButtonLanguage.interactable = false;
                frenchButtonLanguage.interactable = true;
            }

            if (localeID == 1)
            {
                englishButtonLanguage.image.sprite = englishOff;
                frenchButtonLanguage.image.sprite = frenchOn;
                
                englishButtonLanguage.interactable = true;
                frenchButtonLanguage.interactable = false;
            }
            
            StartCoroutine(SetLocale(localeID));
        }
        
        IEnumerator SetLocale(int localeID)
        {
            _active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
            PlayerPrefs.SetInt("LocaleKey", localeID);
            _active = false;
        }
    }
}
