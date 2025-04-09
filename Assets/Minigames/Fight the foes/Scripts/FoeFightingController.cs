using System.Collections;
using System.Collections.Generic;
using Axoloop.Global;
using UnityEngine;

public class FoeFightingController : SingletonMB<FoeFightingController>, IMinigameController
{
    public void GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
    {
        Random.InitState(seed);

        DifficultyMeter difficulty = FoeFightingUtils.SetDifficulty(difficultyLevel);


    }

    

    public void InitializeMinigame()
    {

    }

    public void StartMinigame()
    {
    }


}
