using Axoloop.Global;
using System.Collections;
using UnityEngine;

namespace AxoLoop.Minigames.HitTheRoad
{
    public class HitTheRoadController : SingletonMB<HitTheRoadController>, IMinigameController
    {
        public GameObject road;
        public GameObject turnTrigger;
        public GameObject playerBike;
        public RivalBike rivalBike;
        public MinigameObject hitTheRoad;


        bool easy(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.FirstTime || dif == MinigameDifficultyLevel.VeryEasy
                    || dif == MinigameDifficultyLevel.Easy;
        bool medium(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.Medium || dif == MinigameDifficultyLevel.Hard;
        bool hard(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.VeryHard || dif == MinigameDifficultyLevel.Impossible;

        private void Start()
        {

            if (ScoreManager.Instance != null)
                GenerateMinigame(ScoreManager.Instance.GetCurrentScore(), MinigameHelper.GetDifficulty(hitTheRoad));
            else
                GenerateMinigame(Random.Range(0, 1000), MinigameDifficultyLevel.VeryEasy);
            HitTheRoadSceneManager.Instance.SceneLoaded += (_) => InitializeMinigame(); ;


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
                    rivalBike.setSpeed(20);
                }
                if (medium(difficultyLevel))
                {
                    rivalBike.setSpeed(60);
                }

                if (hard(difficultyLevel))
                {
                    rivalBike.setSpeed(100);
                }
            }


            // générer le mini-jeu (biome et autres paramètres qui influenceront la partie)

        }

        public void InitializeMinigame()
        {
            StartCoroutine(SpawnRivalBike());
        }

        public void StartMinigame()
        {
            // Début officiel du mini-jeu où le joueur peut commencer à jouer
        }

        IEnumerator SpawnRivalBike()
        {
            yield return new WaitForSeconds(Random.Range(0f, 4f)); // Attendre 1 seconde avant de faire apparaître le vélo rival
            if (rivalBike)
            {
                rivalBike.gameObject.SetActive(true);
            }
        }

    }
}
