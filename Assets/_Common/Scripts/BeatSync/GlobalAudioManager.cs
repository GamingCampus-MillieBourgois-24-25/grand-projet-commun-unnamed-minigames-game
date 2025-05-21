using Axoloop.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Common.Scripts
{
    using System.Threading;
    using Unity.VisualScripting;
    using UnityEngine;

    public class GlobalAudioManager : SingletonMB<GlobalAudioManager>
    {

        #region PROPERTIES - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        
        [Header("Music Settings")]
        [SerializeField] private AudioClip musicClip;
        [Range(0f, 100f)]
        [SerializeField] private float volume = 80f;
        [SerializeField] private int audioSourcesPoolSize = 5;


        private AudioSource BGM_AudioSource;
        private AudioSource[] audioSources;
        private Queue<AudioSource> availableSources = new();

        private float savedVolume;

        private bool BM_muted = false;
        private bool SFX_muted = false;
        private bool vibration_muted = false;

        #endregion
        #region LIFECYCLE - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected override void Awake()
        {
            base.Awake(); // Assure que SingletonMB fait son boulot
            SetupBgmAudioSource();
            SetupAudioSourcesPool();
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("BgmOn") == 0)
                MuteMusic();
            if (PlayerPrefs.GetInt("SfxOn") == 0)
                MuteSFX();
            if (PlayerPrefs.GetInt("VibrationOn") == 0)
                MuteVibration();


            if (musicClip != null)
            {
                BGM_AudioSource.Play();
            }
            else
            {
                Debug.LogWarning("MusicManager: Aucun clip musique assigné !");
            }
        }

        #endregion
        #region METHODS - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private void SetupBgmAudioSource()
        {
            BGM_AudioSource = gameObject.AddComponent<AudioSource>();
            BGM_AudioSource.clip = musicClip;
            BGM_AudioSource.loop = true;
            BGM_AudioSource.volume = volume;
            BGM_AudioSource.playOnAwake = false;

            savedVolume = volume;
        }
        private void SetupAudioSourcesPool()
        {
            audioSources = new AudioSource[audioSourcesPoolSize];

            for (int i = 0; i < audioSourcesPoolSize; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                audioSources[i] = source;
                availableSources.Enqueue(source);
            }
        }


        private AudioSource GetAvailableSource()
        {
            if (availableSources.Count > 0)
                return availableSources.Dequeue();

            //no source available, take oldest playing source
            // Unpredictable Behaviour : the previous ReleaseWhenFinished couroutine is still running and will place this audiosource in the queue before this one is released. 
            // This can get out of hands if the the sound is reused multiple times before any previous coroutine is finished, but it should only affect one audiosource. 
            // Increasing the number of available audio sources should mitigate the issue for now.
            float maxTime = 0f;
            AudioSource oldestSource = null;
            foreach (var src in audioSources)
            {
                if(src.time > maxTime)
                {
                    maxTime = src.time;
                    oldestSource = src;
                }
            }
            oldestSource.Stop();
            return oldestSource;

        }

        private IEnumerator<WaitForSeconds> ReleaseWhenFinished(AudioSource source, float duration)
        {
            yield return new WaitForSeconds(duration);
            availableSources.Enqueue(source);
        }


        #endregion
        #region PUBLIC METHODS - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public void PlaySound(AudioClip clip)
        {
            if (SFX_muted || clip == null)
                return;

            BGM_AudioSource.PlayOneShot(clip);



            AudioSource source = GetAvailableSource();
   
            source.volume = volume;
            source.PlayOneShot(clip);

            StartCoroutine(ReleaseWhenFinished(source, clip.length));
        }

        

        /// <summary>
        /// Déclenche une vibration avec un profil donné (nécessite un appareil mobile).
        /// </summary>
        /// <param name="profile">Le type de vibration à déclencher.</param>
        public void Vibrate()
        {
            if (vibration_muted) return;

#if UNITY_ANDROID || UNITY_IOS

            Handheld.Vibrate();

#endif

        }

        public void MuteMusic()
        {
            if (!BM_muted)
            {
                //savedVolume = BGM_AudioSource.volume;
                BGM_AudioSource.volume = 0f;
                BM_muted = true;
            }
        }

        public void UnmuteMusic()
        {
            if (BM_muted)
            {
                BGM_AudioSource.volume = savedVolume;
                BM_muted = false;
            }
        }

        public void MuteSFX()
        {
            if (!SFX_muted)
            {
                //savedVolume = BGM_AudioSource.volume;
                foreach(var source in audioSources)
                {
                    source.volume = 0f;
                } 
                SFX_muted = true;
            }
        }
        public void UnmuteSFX()
        {
            if (SFX_muted)
            {
                foreach (var source in audioSources)
                {
                    source.volume = savedVolume;
                }
                SFX_muted = false;
            }
        }

        public void MuteVibration() => vibration_muted = true;
        public void UnmuteVibration() => vibration_muted = false;


        public void SetVolume(float newVolume)
        {
            volume = Mathf.Clamp01(newVolume);
            if (!BM_muted)
            {
                BGM_AudioSource.volume = volume;
            }
        }

        #endregion
    }

}