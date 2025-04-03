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

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OptionsLoadScene()
    {
        _audioSource.PlayOneShot(soundButton);
        SceneManager.LoadScene(1);
    }
}
