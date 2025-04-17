using Axoloop.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Common.Scripts
{
    [CreateAssetMenu(fileName = "NewBeatProfile", menuName = "BeatSync/BeatProfile")]
    public class BeatProfile : ScriptableObject
    {
        public bool isLooping = false;
        public float baseTempo = 64;
        public List<Beat> beats = new List<Beat>();

        [HideInInspector]
        public bool isDirty = false;
    }

    [System.Serializable]
    public class Beat
    {
        public BeatType type;

        [HideInInspector]
        public float duration;
    }


    public enum BeatType
    {
        Blanche,
        NoirePointée,
        Noire,
        CrochePointée,
        Croche,
        DoubleCroche
    }
}