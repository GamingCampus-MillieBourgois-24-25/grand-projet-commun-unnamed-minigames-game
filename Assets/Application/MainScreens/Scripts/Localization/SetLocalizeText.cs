using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization.Scripts;

namespace Assets.SimpleLocalization
{
	public class SetLocalizeText : MonoBehaviour
	{
		public void SetLocalization(string localization)
		{
			LocalizationManager.Language = localization;
            LocalizationManager._language = localization;
            PlayerPrefs.SetString("Localization", localization);
        }
	}
}