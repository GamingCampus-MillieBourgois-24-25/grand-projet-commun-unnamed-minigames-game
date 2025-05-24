using Axoloop.Global;
using Axoloop.Scripts.Global;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AxoLoop.Minigames.HitTheRoad
{
    public class HitTheRoadController : BaseMinigameController<HitTheRoadController>
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

        public override System.Action OnStartSignal { get; set; }

        bool easy(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.FirstTime || dif == MinigameDifficultyLevel.VeryEasy
                    || dif == MinigameDifficultyLevel.Easy;
        bool medium(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.Medium || dif == MinigameDifficultyLevel.Hard;
        bool hard(MinigameDifficultyLevel dif) => dif == MinigameDifficultyLevel.VeryHard || dif == MinigameDifficultyLevel.Impossible;

        protected override void Awake()
        {
            base.Awake();
            foreach (var item in buttons)
            {
                item.gameObject.SetActive(true);
                item.gameObject.SetActive(false);
            }
        }

        private async void Start()
        {

            if (ScoreManager.Instance)
                await GenerateMinigame(ScoreManager.Instance.GetTotalScore() + 1 * GameSettings.RandomInt, MinigameHelper.GetDifficulty(hitTheRoad));
            else
            {
                await GenerateMinigame(Random.Range(0, 1000), testDifficulty);
                DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(50, 20);
            }


        }

        protected override async Task GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
        {
            if (road)
            {
                Random.InitState(seed);
                GenerateTiles.Instance.check();
                GenerateTiles.Instance.SettingUpTheScene();
                Debug.Log("Seed Controller : " + seed);
            }

            if (rivalBike)
            {
                if (easy(difficultyLevel))
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
            
            await Task.Yield();

            InitializeMinigame();
        }

        protected override void InitializeMinigame()
        {
            SceneLoader.FinishLoading();
            StartMinigame();
        }

        protected override void StartMinigame()
        {
            OnStartSignal += () => StartCoroutine(SpawnRivalBike());
            tutorialText.Enable(OnStartSignal);
        }

        IEnumerator SpawnRivalBike()
        {
            yield return new WaitForSeconds(Random.Range(0.8f, 2.5f));
            GenerateTiles.Instance.ShowOnomatopea();

            yield return new WaitForSeconds(0.3f);
            PlayerBike.Instance.PlaySpotAnimation();

            yield return new WaitForSeconds(0.2f);
            SpawnButtons();

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

        void SpawnButtons()
        {
            foreach (var item in buttons)
            {
                item.gameObject.SetActive(true);

                // make a dotween animation to scale up item
                item.transform.localScale = Vector3.zero;
                item.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBounce).SetAutoKill();
            }
        }
    }
}
