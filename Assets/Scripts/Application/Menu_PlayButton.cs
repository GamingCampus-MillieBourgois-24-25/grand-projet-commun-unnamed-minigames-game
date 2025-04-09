using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(5);
    }
}
