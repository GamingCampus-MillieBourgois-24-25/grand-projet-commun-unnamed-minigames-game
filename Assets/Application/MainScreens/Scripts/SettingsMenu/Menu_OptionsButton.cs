using UnityEngine;
using Assets.Code.GLOBAL;
using Axoloop.Global;

public class Menu_OptionsButton : MonoBehaviour
{
    public AudioClip soundButton;
    private AudioSource _audioSource;
    public bool optionOpen;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        optionOpen = false;
    }

    public void OptionsLoadScene()
    {
        if (optionOpen == false)
        {
            _audioSource.PlayOneShot(soundButton);
            GlobalSceneController.OpenScene(GameSettings.SettingsScene.name);
            optionOpen = true;
        }
    }
}
