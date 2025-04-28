using System;
using System.Collections;
using Axoloop.Global;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingController : SingletonMB<FoeFightingController>, IMinigameController
    {

        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        [SerializeField] ButtonController[] attackButtons;
        [SerializeField] ButtonController blockButton;

        [SerializeField] GameObject FoeSpawnPoint;
        [SerializeField] GameObject AxoSpawnPoint;

        [SerializeField] GameObject AxoIntro;
        [SerializeField] Transform AxoIntroTop;

        [SerializeField] CanvasGroup ButtonsCanvasGroup;

        [SerializeField] ContinueText ContinueText;
        public MinigameObject fightTheFoes;

        DifficultyMeter difficulty;

        public bool canBegin = true;

        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------

        public void Start()
        {
            if (ScoreManager.Instance != null)
                GenerateMinigame(ScoreManager.Instance.GetTotalScore()+1*GameSettings.RandomInt, MinigameHelper.GetDifficulty(fightTheFoes));
            else
                GenerateMinigame(UnityEngine.Random.Range(0, 1000), MinigameDifficultyLevel.VeryEasy);

            StartMinigame();

        }

        #endregion
        #region METHODS-------------------------------------------------------------------------

        public void GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
        {
            UnityEngine.Random.InitState(seed);
            difficulty = FoeFightingUtils.SetDifficulty(difficultyLevel);


            FoeFightMinigameData.GameFoes = FoeFightingUtils.GenerateEnnemies(FoeFightMinigameData.FoesList, 2);
            RageBar.Instance.SetFillSpeed(difficulty);
            FoeFightingManager.Instance.SetEnvironmentVariant((UnityEngine.Random.value > 0.5f));
            FoeFightingManager.Instance.SetLogoVariant((UnityEngine.Random.value > 0.5f));
        }

        public void InitializeMinigame()
        {
            
        }

        public void StartMinigame()
        {
            FoeFightMinigameData.LockedAttack = true;
            RageBar.Instance.StopFill();

            StartCoroutine(AxoAnimation(() => NextRound()));
        }



        void NextRound()
        {
            if (FoeFightMinigameData.GameFoes.Count > 0)
            {
                StartCoroutine(SpawnFoe(() => BeginTurn()));
            }
            else
            {
                ContinueText.Enable(true);
                MinigameHelper.IncrementMinigamePlayed(fightTheFoes);
            }
        }

        public void BeginTurn()
        {
            if (!canBegin) return;
            canBegin = false;

            ButtonsCanvasGroup.alpha = 1;
            FoeFightMinigameData.CurrentAttacks = FoeFightingUtils.ShuffleAttacks(difficulty, FoeFightMinigameData.AttackObjectList);
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
            Vector2 axoIntroBasePosition = AxoIntro.transform.position;
            float time = 2f;
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Sin((elapsedTime / time) * Mathf.PI); // Sinusoidal interpolation  
                AxoIntro.transform.position = Vector2.Lerp(axoIntroBasePosition, AxoIntroTop.position, t);
                yield return null;
            }
            AxoIntro.SetActive(false);
            yield return new WaitForSeconds(0.5f);



            Vector2 spawnPosition = new Vector2(AxoSpawnPoint.transform.position.x, AxoSpawnPoint.transform.position.y - 15);
            FoeFightMinigameData.Axo.transform.position = spawnPosition;
            FoeFightMinigameData.Axo.gameObject.SetActive(true);
            time = 0.8f;
            elapsedTime = 0f;
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                FoeFightMinigameData.Axo.transform.position = Vector2.Lerp(spawnPosition, AxoSpawnPoint.transform.position, elapsedTime / time);
                yield return null;
            }


            yield return new WaitForSeconds(0.5f);
            callback?.Invoke();
        }


        private IEnumerator SpawnFoe(Action callback)
        {
            Vector2 spawnPosition = new Vector2(FoeSpawnPoint.transform.position.x + 10, FoeSpawnPoint.transform.position.y);

            FoeFightMinigameData.CurrentFoe = Instantiate(FoeFightMinigameData.GameFoes[0], spawnPosition, Quaternion.identity, FoeSpawnPoint.transform);

            float time = 1f;
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