using Assets._Common.Scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Button bgmButton, sfxButton, vibrationButton, resetSave;
    public Sprite bgmOnSprite, bgmOffSprite;
    public Sprite sfxOnSprite, sfxOffSprite;
    public Sprite vibrationOnSprite, vibrationOffSprite;
    public Toggle colorblindToggle;


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
        if(_bgmOn)
        {
            GlobalAudioManager.Instance.UnmuteMusic();
        }
        else
        {
            GlobalAudioManager.Instance.MuteMusic();
        }

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
        PlayerPrefs.SetInt("VibrationOn", _vibrationOn ? 1 : 0);
        PlayerPrefs.Save();
        if (_vibrationOn) Handheld.Vibrate();
        UpdateButtons();
    }

    public void ToggleColorblind()
    {
        string value = colorblindToggle.isOn ? "True" : "False";
        PlayerPrefs.SetString("ColorblindMode", value);
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("BgmOn", 1);
        PlayerPrefs.SetInt("SfxOn", 1);
        PlayerPrefs.SetInt("VibrationOn", 1);
        PlayerPrefs.SetString("ColorblindMode", "False");
        PlayerPrefs.Save();
        LoadSettings();
        UpdateButtons();

        SceneManager.LoadScene("MAIN_MainScene");
    }

    private void LoadSettings()
    {
        if (!PlayerPrefs.HasKey("BgmOn"))
        {
            PlayerPrefs.SetInt("BgmOn", 1);
            PlayerPrefs.SetInt("SfxOn", 1);
            PlayerPrefs.SetInt("VibrationOn", 1);
            PlayerPrefs.SetString("ColorblindMode", "False");
        }
        _bgmOn = PlayerPrefs.GetInt("BgmOn", 1) == 1;
        _sfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;
        _vibrationOn = PlayerPrefs.GetInt("VibrationOn", 1) == 1;
        colorblindToggle.isOn = PlayerPrefs.GetString("ColorblindMode", "False") == "True";

        audioMixer.SetFloat("BGMVolume", _bgmOn ? 0f : -80f);
        audioMixer.SetFloat("SFXVolume", _sfxOn ? 0f : -80f);
        PlayerPrefs.Save();
    }

    private void UpdateButtons()
    {
        bgmButton.image.sprite = _bgmOn ? bgmOnSprite : bgmOffSprite;
        sfxButton.image.sprite = _sfxOn ? sfxOnSprite : sfxOffSprite;
        vibrationButton.image.sprite = _vibrationOn ? vibrationOnSprite : vibrationOffSprite;
    }
}
