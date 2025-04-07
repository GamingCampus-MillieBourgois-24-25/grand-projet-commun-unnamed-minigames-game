using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_OptionsButton : MonoBehaviour
{
    public AudioClip soundButton;
    private AudioSource _audioSource;
    public Boolean optionOpen;

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
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            optionOpen = true;
        }
    }
}
