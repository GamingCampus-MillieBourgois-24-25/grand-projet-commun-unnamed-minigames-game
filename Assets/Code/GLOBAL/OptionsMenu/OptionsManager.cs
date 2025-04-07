using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Button bgmButton, sfxButton, vibrationButton;
    public Sprite bgmOnSprite, bgmOffSprite;
    public Sprite sfxOnSprite, sfxOffSprite;
    public Sprite vibrationOnSprite, vibrationOffSprite;
    
    private bool _bgmOn = false;
    private bool _sfxOn = false;
    private bool _vibrationOn = false;

    private void Start()
    {
        LoadSettings();
        UpdateButtons();
    }
    
    public void ToggleBgm()
    {
        _bgmOn = !_bgmOn;
        PlayerPrefs.SetInt("BgmOn", _bgmOn ? 1 : 0);
        PlayerPrefs.Save();
        audioMixer.SetFloat("BGMVolume", _bgmOn ? 0f : -80f);
        UpdateButtons();
    }

    public void ToggleSfx()
    {
        _sfxOn = !_sfxOn;
        PlayerPrefs.SetInt("SfxOn", _sfxOn ? 1 : 0);
        PlayerPrefs.Save();
        audioMixer.SetFloat("SFXVolume", _sfxOn ? 0f : -80f);
        UpdateButtons();
    }

    public void ToggleVibration()
    {
        _vibrationOn = !_vibrationOn;
        PlayerPrefs.SetInt("Vibration", _vibrationOn ? 1 : 0);
        PlayerPrefs.Save();
        if (_vibrationOn) Handheld.Vibrate();
        UpdateButtons();
    }

    private void LoadSettings()
    {
        _bgmOn = PlayerPrefs.GetInt("BgmOn", 1) == 1;
        _sfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;
        _vibrationOn = PlayerPrefs.GetInt("VibrationOn", 1) == 1;
        
        audioMixer.SetFloat("BGMVolume", _bgmOn ? 0f : -80f);
        audioMixer.SetFloat("SFXVolume", _sfxOn ? 0f : -80f);
    }
    
    private void UpdateButtons()
    {
        bgmButton.image.sprite = _bgmOn ? bgmOnSprite : bgmOffSprite;
        sfxButton.image.sprite = _sfxOn ? sfxOnSprite : sfxOffSprite;
        vibrationButton.image.sprite = _vibrationOn ? vibrationOnSprite : vibrationOffSprite;
    }
}
