using System.Collections.Generic;
using Axoloop.Global;
using JetBrains.Annotations;
using UnityEngine;

namespace AxoLoop.Minigames.HitTheRoad
{
    public class HitTheRoadController : SingletonMB<HitTheRoadController>, IMinigameController
    {
        public GameObject road;
        public GameObject turnTrigger;
        public GameObject playerBike;
        public GameObject rivalBike;

        bool easy(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.FirstTime || dif == MinigameDifficultyLevel.VeryEasy
                    || dif == MinigameDifficultyLevel.Easy;
        bool medium(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.Medium || dif == MinigameDifficultyLevel.Hard;
        bool hard(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.VeryHard || dif == MinigameDifficultyLevel.Impossible;

        private void Start()
        {
            GenerateMinigame(Random.Range(0, 1000), MinigameDifficultyLevel.VeryEasy);
            InitializeMinigame();


        }

        public void GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
        {
            if (road)
            {
                Random.InitState(seed);
                road.GetComponent<GenerateTiles>().check();
                road.GetComponent<GenerateTiles>().SettingUpTheScene();
                Debug.Log("Seed Controller : " + seed);
            }

            if (rivalBike)
            {
                if(easy(difficultyLevel))
                {
                    rivalBike.GetComponent<RivalBike>().setSpeed(20);
                }
                if (medium(difficultyLevel))
                {
                    rivalBike.GetComponent<RivalBike>().setSpeed(60);
                }

                if (hard(difficultyLevel))
                {
                    rivalBike.GetComponent<RivalBike>().setSpeed(100);
                }
            }


            // générer le mini-jeu (biome et autres paramètres qui influenceront la partie)

        }

        public void InitializeMinigame()
        {
            // Si nécessaire, initialiser des éléments du minijeu
        }

        public void StartMinigame()
        {
            // Début officiel du mini-jeu où le joueur peut commencer à jouer
        }

    }
}
