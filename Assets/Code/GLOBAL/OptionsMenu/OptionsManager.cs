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
        UpdateButtons();
    }
    
    public void ToggleBgm()
    {
        _bgmOn = !_bgmOn;
        audioMixer.SetFloat("BGMVolume", _bgmOn ? 0f : -80f);
        UpdateButtons();
    }

    public void ToggleSfx()
    {
        _sfxOn = !_sfxOn;
        audioMixer.SetFloat("SFXVolume", _sfxOn ? 0f : -80f);
        UpdateButtons();
    }

    public void ToggleVibration()
    {
        _vibrationOn = !_vibrationOn;
        PlayerPrefs.SetInt("Vibration", _vibrationOn ? 1 : 0);
        Handheld.Vibrate();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bgmButton.image.sprite = _bgmOn ? bgmOnSprite : bgmOffSprite;
        sfxButton.image.sprite = _sfxOn ? sfxOnSprite : sfxOffSprite;
        vibrationButton.image.sprite = _vibrationOn ? vibrationOnSprite : vibrationOffSprite;
    }
}
