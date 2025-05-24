using System;
using System.Collections;
using System.Threading.Tasks;
using Axoloop.Global;
using Axoloop.Scripts.Global;
using DG.Tweening;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingController : BaseMinigameController<FoeFightingController>
    {

        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------


        [SerializeField] MinigameDifficultyLevel testDifficulty;

        [SerializeField] ButtonController[] attackButtons;
        [SerializeField] ButtonController blockButton;

        [SerializeField] GameObject FoeSpawnPoint;
        [SerializeField] GameObject AxoSpawnPoint;

        [SerializeField] GameObject AxoIntro;
        [SerializeField] Transform AxoIntroTop;

        [SerializeField] CanvasGroup ButtonsCanvasGroup;

        [SerializeField] ContinueText ContinueText;
        public MinigameObject fightTheFoes;

        public bool canBegin = true;
        public override System.Action OnStartSignal { get; set; }


        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------

        public async void Start()
        {
            if (ScoreManager.Instance != null)
                await GenerateMinigame(ScoreManager.Instance.GetTotalScore()+1*GameSettings.RandomInt, MinigameHelper.GetDifficulty(fightTheFoes));
            else
                await GenerateMinigame(UnityEngine.Random.Range(0, 1000), testDifficulty);


        }

        #endregion
        #region METHODS-------------------------------------------------------------------------

        protected override async Task GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
        {
            UnityEngine.Random.InitState(seed);

            FoeFightMinigameData.Difficulty = FoeFightingUtils.SetDifficulty(difficultyLevel);
            FoeFightMinigameData.GameFoes = FoeFightingUtils.GenerateEnnemies(FoeFightMinigameData.FoesList, 2, FoeFightMinigameData.Difficulty);

            FoeFightingManager.Instance.SetEnvironmentVariant((UnityEngine.Random.value > 0.5f));
            FoeFightingManager.Instance.SetLogoVariant((UnityEngine.Random.value > 0.5f));
            
            RageBar.Instance.SetFillSpeed(FoeFightMinigameData.Difficulty);

            InitializeMinigame();
        }

        protected override void InitializeMinigame()
        {
            SceneLoader.FinishLoading();
            StartMinigame();
        }

        protected override void StartMinigame()
        {
            FoeFightMinigameData.CurrentTurn = 0;
            FoeFightMinigameData.LockedAttack = true;
            RageBar.Instance.StopFill();

            StartCoroutine(AxoAnimation(NextRound));
        }



        void NextRound()
        {
            if (FoeFightMinigameData.GameFoes.Count > 0)
            {
                StartCoroutine(SpawnFoe(() => BeginTurn()));
            }
            else
            {
                MiniGameManager.Instance?.PlayEndSound(true);
                ContinueText.Enable(true);
                MinigameHelper.IncrementMinigamePlayed(fightTheFoes);
            }
        }

        public void BeginTurn()
        {
            FoeFightMinigameData.CurrentTurn++;
            if (!canBegin) return;
            canBegin = false;

            ButtonsCanvasGroup.alpha = 1;
            FoeFightMinigameData.CurrentAttacks = FoeFightingUtils.ShuffleAttacks(FoeFightMinigameData.Difficulty, FoeFightMinigameData.AttackObjectList, FoeFightMinigameData.CurrentTurn);
            for (int i = 0; i < 2; i++)
            {
                attackButtons[i].SetButtonData(FoeFightMinigameData.CurrentAttacks[i]);
                attackButtons[i].enabled = true;
            }
            FoeFightMinigameData.LockedAttack = false;
            FoeFightMinigameData.IsBlocking = false;

            FoeFightingUtils.ButtonsEnter?.Invoke();
            RageBar.Instance.ResumeFill();

        }

        void GameOver()
        {
            ContinueText.Enable(false);
        }

        #endregion
        #region API-----------------------------------------------------------------------------
        public void FoeTurn()
        {
            Action next = FoeFightMinigameData.IsBlocking ? BeginTurn : GameOver;

            FoeFightMinigameData.CurrentFoe.AttackAnimation(next);
        }

        public void CurrentFoeKilled()
        {
            FoeFightMinigameData.GameFoes.RemoveAt(0);
            StartCoroutine(DespawnFoe(() => NextRound()));

        }

        #endregion
        #region COROUTINES----------------------------------------------------------------------

        private IEnumerator AxoAnimation(Action callback)
        {
            // make axointro move upward then downward in 2.5 seconds : 
            AxoIntro.SetActive(true);
            AxoIntro.transform.DOMove(AxoIntroTop.position, 1.2f)
                .From()
                .SetEase(Ease.InBack)
                .OnComplete(() => tutorialText.Enable(OnStartSignal));

            yield return new WaitForSeconds(1.5f);

            Vector2 spawnPosition = new Vector2(AxoSpawnPoint.transform.position.x, AxoSpawnPoint.transform.position.y - 15);
            FoeFightMinigameData.Axo.gameObject.SetActive(true);
            FoeFightMinigameData.Axo.transform
                .DOMove(spawnPosition, 1.5f)
                .From()
                .SetEase(Ease.OutQuart);

            yield return new WaitForSeconds(2f);
            callback?.Invoke();
        }


        private IEnumerator SpawnFoe(Action callback)
        {
            Vector2 spawnPosition = new Vector2(FoeSpawnPoint.transform.position.x + 7, FoeSpawnPoint.transform.position.y);

            FoeFightMinigameData.CurrentFoe = Instantiate(FoeFightMinigameData.GameFoes[0], spawnPosition, Quaternion.identity, FoeSpawnPoint.transform);

            float time = 0.7f;
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                FoeFightMinigameData.CurrentFoe.transform.position = Vector2.Lerp(spawnPosition, FoeSpawnPoint.transform.position, elapsedTime / time);
                yield return null;
            }
            FoeFightMinigameData.CurrentFoe.transform.position = FoeSpawnPoint.transform.position;
            canBegin = true;
            callback?.Invoke();
        }

        private IEnumerator DespawnFoe(Action callback)
        {
            Vector2 exitPosition = new Vector2(FoeSpawnPoint.transform.position.x + 10, FoeSpawnPoint.transform.position.y);
            float time = 1f;
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                FoeFightMinigameData.CurrentFoe.transform.position = Vector2.Lerp(FoeSpawnPoint.transform.position, exitPosition, elapsedTime / time);
                yield return null;
            }
            Destroy(FoeFightMinigameData.CurrentFoe.gameObject);
            callback?.Invoke();
        }
        #endregion
    }
}