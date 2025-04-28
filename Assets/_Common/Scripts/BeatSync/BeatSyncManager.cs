using Axoloop.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle;
using UnityEngine;

namespace Assets._Common.Scripts
{
    public class BeatSyncManager : SingletonMB<BeatSyncManager>
    {
        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        [SerializeField] BeatProfile _mainBeatProfile;
        [SerializeField] float _speedMultiplier = 1f;

        public event Action OnBeat;
        


        private List<Coroutine> _playingCoroutines = new List<Coroutine>();

        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------


        private void Start()
        {
            Coroutine coroutine = StartCoroutine(PlayBeatProfile(_mainBeatProfile));
        }

        #endregion
        #region METHODS-------------------------------------------------------------------------


        #endregion

        IEnumerator PlayBeatProfile(BeatProfile profile)
        {
            bool looping = profile.isLooping;
            do
            {
                float time = 0;

                for (int i = 0; i < profile.beats.Count; i++)
                {
                    Beat beat = profile.beats[i];
                    //OnBeat?.Invoke(beat.type);

                    // Calcule le temps absolu où ce beat doit se jouer
                    float beatTime = beat.duration / _speedMultiplier;

                    // Attend jusqu’à ce temps, en se resynchronisant à chaque frame
                    while (time < beatTime)
                    {
                        time += Time.deltaTime;
                        yield return null;
                    }
                    OnBeat?.Invoke();
                }
                yield return null;

            } while (looping);
        }
        
    }
}