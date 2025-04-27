using Axoloop.Global;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AxoLoop.Minigames.HitTheRoad
{
    public class HitTheRoadController : SingletonMB<HitTheRoadController>, IMinigameController
    {
        public GameObject road;
        public GameObject turnTrigger;
        public GameObject playerBike;
        public RivalBike rivalBike;
        public MinigameObject hitTheRoad;
        public Button[] buttons;

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


            // g�n�rer le mini-jeu (biome et autres param�tres qui influenceront la partie)

        }

        public void InitializeMinigame()
        {
            StartCoroutine(SpawnRivalBike());
        }

        public void StartMinigame()
        {
            // D�but officiel du mini-jeu o� le joueur peut commencer � jouer
        }

        IEnumerator SpawnRivalBike()
        {
            yield return new WaitForSeconds(Random.Range(1f, 4f)); // Attendre 1 seconde avant de faire appara�tre le v�lo rival
            if (rivalBike)
            {
                rivalBike.gameObject.SetActive(true);
            }
        }

        public void DisableButtons()
        {
            foreach (var item in buttons)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
