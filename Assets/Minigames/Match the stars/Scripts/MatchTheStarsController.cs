
using Assets._Common.Scripts;
using Assets.Minigames.Match_the_stars.Scripts;
using Assets.Scripts.GLOBAL;
using Axoloop.Global;
using Axoloop.Scripts.Global;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace AxoLoop.Minigames.MatchTheStars
{
    public class MatchTheStarsController : SingletonMB<MatchTheStarsController>, IMinigameController
    {

        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        [SerializeField] private MinigameObject matchTheStars;
        [SerializeField] GameObject[] wallPosters;
        [SerializeField] private StarsOnCrown playerCrown;
        [SerializeField] AxoMTS axo;
        [SerializeField] OpenBag openBag;
        [SerializeField] ContinueText continueText;

        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------

        private async void Start()
        {
            if (ScoreManager.Instance != null)
                await GenerateMinigame(ScoreManager.Instance.GetTotalScore() + 1 * GameSettings.RandomInt, MinigameHelper.GetDifficulty(matchTheStars));
            else
                await GenerateMinigame(UnityEngine.Random.Range(0, 1000), MinigameDifficultyLevel.VeryEasy);

            MatchingTheStarsSceneManager.Instance.SceneLoaded += (_) => StartMinigame(); ;
        }

        #endregion
        #region METHODS-------------------------------------------------------------------------

        public async Task GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
        {
            UnityEngine.Random.InitState(seed);
            MatchTheStarsMinigameData.Difficulty = MTSUtils.SetDifficulty(difficultyLevel);

            await StarsColorsGenerator.SetStarsColor();
            StarsColorsGenerator.SetThreeStars();

            wallPosters[Random.Range(0, wallPosters.Length)].SetActive(true);


            InitializeMinigame();
        }

        public void InitializeMinigame()
        {
            SceneLoader.FinishLoading();
        }

        public void StartMinigame()
        {
           
        }


        public void StartVerification()
        {
            StartCoroutine(EndAnimation(checkWin()));
        }


        #endregion
        #region API-----------------------------------------------------------------------------
       
        public bool checkWin()
        {
            var testA = MatchTheStarsMinigameData.CrownStarsImages;
            var testB = playerCrown.GetPlayerCrownSprites();

            var allMatch = true;
            for (int i = 0; i < testA.Length; i++)
            {
                if (testA[i].sprite.name != testB[i].name) // Comparaison des noms des sprites
                {
                    allMatch = false;
                    break;
                }
            }

            return allMatch;
        }


        #endregion
        #region COROUTINES----------------------------------------------------------------------


        IEnumerator EndAnimation(bool win)
        {
            yield return new WaitForSeconds(0.5f);
            openBag.ForceCloseTheBag();
            openBag.enabled = false;
            yield return new WaitForSeconds(1f);

            MiniGameManager.Instance?.PlayEndSound(win);

            if (win)
            {
                axo.SetHappy();
            }
            else
            {
                axo.SetAngry();
            }

            continueText.Enable(win);
        }

        #endregion
    }
}