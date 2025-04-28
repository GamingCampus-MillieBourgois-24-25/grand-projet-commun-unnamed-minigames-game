using Axoloop.Global;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle;
using UnityEngine;

namespace Assets._Common.Scripts
{
    public class BeatSyncListener : MonoBehaviour
    {
        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        [SerializeField] BeatBehaviour beatBehaviour;
        [SerializeField] float beatStrengthAmount = 1.03f;
        [SerializeField] float posMult = 20;

        bool yoyoUp = true;
        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------

        private void OnEnable()
        {
            if(BeatSyncManager.Instance == null)
            {
                Debug.Log("BeatSyncManager is not initialized.");
                return;
            }
            BeatSyncManager.Instance.OnBeat += OnBeat;
        }

        private void OnDisable()
        {
            if(BeatSyncManager.Instance == null)
            {
                Debug.Log("BeatSyncManager is not initialized.");
                return;
            }
            BeatSyncManager.Instance.OnBeat -= OnBeat;

        }

        private void Start()
        {
        }

        #endregion
        #region METHODS-------------------------------------------------------------------------

        public void SetBeatBehaviour(BeatBehaviour newBehaviour)
        {
            beatBehaviour = newBehaviour;
            Debug.Log("" + newBehaviour);
        }

        void OnBeat()
        {
            switch(beatBehaviour)
            {
                case BeatBehaviour.ScaleBeat:
                    PlayScaleBeat();
                    break;
                case BeatBehaviour.VerticalScaleBeat:
                    PlayVerticalScaleBeat();
                    break;
                case BeatBehaviour.BounceBeat:
                    PlayBounceBeat();
                    break;
                case BeatBehaviour.ShakeBeat:
                    PlayShakeBeat();
                    break;
                case BeatBehaviour.YoyoBeat:
                    PlayYoyoBeat();
                    break;
            }
        }


        void PlayScaleBeat()
        {
            transform.DOScale(transform.localScale * beatStrengthAmount, 0.15f).SetLoops(2, LoopType.Yoyo);
        }
        void PlayVerticalScaleBeat()
        {
            transform.DOScaleY(transform.localScale.y * beatStrengthAmount, 0.15f).SetLoops(2, LoopType.Yoyo);
        }
        void PlayBounceBeat()
        {
            transform.DOLocalMoveY(transform.localPosition.y + beatStrengthAmount* posMult, 0.15f).SetLoops(2, LoopType.Yoyo);
        }
        void PlayShakeBeat()
        {
            transform.DOShakePosition(0.15f, beatStrengthAmount* posMult);
        }
        void PlayYoyoBeat()
        {
            yoyoUp = !yoyoUp;
            int dir = yoyoUp ? 1 : -1;
            transform.DOLocalMoveY(dir*beatStrengthAmount*posMult, 0.2f);
        }

        #endregion

        public enum BeatBehaviour
        {
            ScaleBeat,
            VerticalScaleBeat,
            BounceBeat,
            ShakeBeat,
            YoyoBeat
        }
        
    }
}