using System.Collections.Generic;
using Axoloop.Global;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingController : SingletonMB<FoeFightingController>, IMinigameController
    {
        [SerializeField] ButtonController[] attackButtons;
        [SerializeField] ButtonController blockButton;
        [SerializeField] GameObject spawnPoint;

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
        }

        public void StartMinigame()
        {
            NextRound();
        }



        public void NextRound()
        {
            if (FoeFightMinigameData.GameFoes.Count > 0)
            {
                FoeFightMinigameData.CurrentFoe = Instantiate(FoeFightMinigameData.GameFoes[0], spawnPoint.transform.position, Quaternion.identity);                    
            }
            else
            {
                // no more ennemies : win
            }
            BeginTurn();
        }

        void BeginTurn()
        {
            FoeFightMinigameData.CurrentAttacks = FoeFightingUtils.ShuffleAttacks(difficulty, FoeFightMinigameData.AttackList);
            for (int i = 0; i < 2; i++)
            {
                attackButtons[i].SetButtonType(FoeFightMinigameData.CurrentAttacks[i]);
                attackButtons[i].enabled = true;
            }
        }

        public void FoeTurn(bool blocking)
        {
            //foe attack

            if (!blocking)
            {
                //game over
            }
            else
            {
                // tank
            }

            BeginTurn();//nextTurn
        }
    }
}