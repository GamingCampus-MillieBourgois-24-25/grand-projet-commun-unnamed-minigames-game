using Axoloop.Global;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AxoLoop.Minigames.HitTheRoad
{
    public class HitTheRoadController : SingletonMB<HitTheRoadController>, IMinigameController
    {
        [SerializeField] MinigameDifficultyLevel testDifficulty;

        public GameObject road;
        public GameObject turnTrigger;
        public GameObject playerBike;
        public RivalBike rivalBike;
        public MinigameObject hitTheRoad;
        public Button[] buttons;
        public bool invertButtons;

        public bool loose = false;

        bool easy(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.FirstTime || dif == MinigameDifficultyLevel.VeryEasy
                    || dif == MinigameDifficultyLevel.Easy;
        bool medium(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.Medium || dif == MinigameDifficultyLevel.Hard;
        bool hard(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.VeryHard || dif == MinigameDifficultyLevel.Impossible;

        private void Start()
        {

            if (ScoreManager.Instance != null)
                GenerateMinigame(ScoreManager.Instance.GetTotalScore() + 1 * GameSettings.RandomInt, MinigameHelper.GetDifficulty(hitTheRoad));
            else
                GenerateMinigame(Random.Range(0, 1000), testDifficulty);
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
                    rivalBike.setSpeed(12);
                }
                if (medium(difficultyLevel))
                {
                    rivalBike.setSpeed(Random.Range(13f, 15f));
                }

                if (hard(difficultyLevel))
                {
                    rivalBike.setSpeed(Random.Range(16f, 18f));
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
            yield return new WaitForSeconds(Random.Range(1f, 3f)); // Attendre 1 seconde avant de faire apparaître le vélo rival
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
