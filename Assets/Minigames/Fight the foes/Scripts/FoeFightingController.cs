using System.Collections.Generic;
using Axoloop.Global;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingController : SingletonMB<FoeFightingController>, IMinigameController
    {
        DifficultyMeter difficulty;
        public void GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
        {
            Random.InitState(seed);

           difficulty = FoeFightingUtils.SetDifficulty(difficultyLevel);

            FoeFightMinigameData.GameFoes = FoeFightingUtils.GenerateEnnemies(FoeFightMinigameData.FoesList, 3);
        }

        public void InitializeMinigame()
        {
            FoeFightMinigameData.Axo.Spawn(null);
            FoeFightingUtils.EnnemySpawn(FoeFightMinigameData.GameFoes);
        }

        public void StartMinigame()
        {
            FoeFightingUtils.ShuffleAttacks(difficulty, FoeFightMinigameData.AttackList);
            //enable inputs
        }

    }
}