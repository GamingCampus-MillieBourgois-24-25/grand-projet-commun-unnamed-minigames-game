using Axoloop.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Common.Scripts
{
    using UnityEngine;

    public class MusicManager : SingletonMB<MusicManager>
    {
        [Header("Music Settings")]
        [SerializeField] private AudioClip musicClip;
        [Range(0f, 1f)]
        [SerializeField] private float volume = 1f;

        private AudioSource audioSource;
        private bool isMuted = false;
        private float savedVolume;

        protected override void Awake()
        {
            base.Awake(); // Assure que SingletonMB fait son boulot
            SetupAudioSource();
        }

        private void SetupAudioSource()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.playOnAwake = false;

            savedVolume = volume;
        }

        private void Start()
        {
            if (musicClip != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("MusicManager: Aucun clip assigné !");
            }
        }

        public void MuteMusic()
        {
            if (!isMuted)
            {
                savedVolume = audioSource.volume;
                audioSource.volume = 0f;
                isMuted = true;
            }
        }

        public void UnmuteMusic()
        {
            if (isMuted)
            {
                audioSource.volume = savedVolume;
                isMuted = false;
            }
        }

        public void SetVolume(float newVolume)
        {
            volume = Mathf.Clamp01(newVolume);
            if (!isMuted)
            {
                audioSource.volume = volume;
            }
        }
    }

}