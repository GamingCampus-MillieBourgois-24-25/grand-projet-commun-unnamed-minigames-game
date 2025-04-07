using System;
using System.Collections;
using UnityEngine;

namespace Axoloop.Application.Option
{
    public class LocaleSelector : MonoBehaviour
    {
        //private bool _active = false;

        //private void Start()
        //{
        //    var id = PlayerPrefs.GetInt("LocaleKey", 0);
        //    ChangeLocale(id);
        //}

        //public void ChangeLocale(int localeID)
        //{
        //    if (_active == true) return;
        //    StartCoroutine(SetLocale(localeID));
        //}
        
        //IEnumerator SetLocale(int localeID)
        //{
        //    _active = true;
        //    yield return LocalizationSettings.InitializationOperation;
        //    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        //    PlayerPrefs.SetInt("LocaleKey", localeID);
        //    _active = false;
        //}
    }
}
