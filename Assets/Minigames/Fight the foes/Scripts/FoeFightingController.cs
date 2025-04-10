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

        List<Foe> gameFoes = FoeFightingUtils.GenerateEnnemies(FoeFightMinigameData.FoesList, 3);
    }

    

    public void InitializeMinigame()
    {

    }

    public void StartMinigame()
    {
    }


}
