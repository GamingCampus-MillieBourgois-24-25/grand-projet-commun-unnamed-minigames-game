using UnityEngine;
using Assets.Code.GLOBAL;
using Axoloop.Global;

public class Menu_PlayButton : MonoBehaviour
{
    public AudioClip soundPlay;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayLoadScene()
    {
        _audioSource.PlayOneShot(soundPlay);
        GlobalSceneController.OpenScene(GameSettings.MainMenuScene.name);
    }
}
