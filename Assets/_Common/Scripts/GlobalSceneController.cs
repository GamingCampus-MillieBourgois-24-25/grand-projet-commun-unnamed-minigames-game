using Axoloop.Global;
using Axoloop.Global.UI;
using Axoloop.Scripts.Global;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.GLOBAL
{
    public class GlobalSceneController : SingletonMB<GlobalSceneController>
    {
        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        [SerializeField] UIObjectGroup _LoaderObject;
        [SerializeField] UIObjectGroup _BlackScreen;

        private ISceneManager _loadedScene;
        private ISceneManager _loadedOverlayScene;

        public Action onHideLoader;

        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------

        private void Start()
        {
            InitialLoading();
        }

        #endregion
        #region METHODS-------------------------------------------------------------------------

        // charger la scène de départ au démarrage du jeu
        void InitialLoading()
        {
            if (GameSettings.IsTesting)
            {
                OpenScene(GameSettings.TestScene, true);
            }
            else
            {
                OpenScene(GameSettings.StartScene);
            }
        }

        // logique post chargement de la scène
        void SceneLoadedHandler(string sceneName)
        {
            Scene loadedScene = SceneManager.GetSceneByName(sceneName);

            ISceneManager loadedSceneManager = FindSceneManagerInstance(loadedScene);

            if (loadedSceneManager == null)
            {
                Debug.LogError($"Aucun ISceneManager trouvé dans la scène {sceneName}");
                return;
            }

            UnloadOverlayScene();

            switch (loadedSceneManager.SceneLevel)
            {
                case SceneLevel.Level1:
                    _loadedScene?.UnloadScene();
                    _loadedScene = loadedSceneManager;
                    break;
                case SceneLevel.Level2:
                    _loadedOverlayScene = loadedSceneManager;
                    break;
            }


            HideLoader();
        }


        /// <summary>
        /// Trouve le SceneManager dans une scène chargée donnée
        /// </summary>
        ISceneManager FindSceneManagerInstance(Scene targetScene)
        {
            ISceneManager sceneManager = null;
            if (targetScene.IsValid())
            {
                GameObject[] rootGameObjects = targetScene.GetRootGameObjects();
                foreach (GameObject rootGameObject in rootGameObjects)
                {
                    sceneManager = rootGameObject.GetComponent<ISceneManager>();
                    if (sceneManager != null)
                    {
                        break;
                    }
                }
            }
            return sceneManager;
        }


        void UnloadFrontScene()
        {
            _loadedScene?.UnloadScene();
        }

        #endregion
        #region API-----------------------------------------------------------------------------

        /// <summary>
        /// Ouvre n'importe quelle scène. <br></br>
        /// Si la scène est de niveau 1, elle sera ouverte en arrière plan et la scène actuelle sera déchargée.<br></br>
        /// Si la scène est de niveau 2, elle sera ouverte immédiatement.
        /// </summary>
        /// <param name="targetScene"></param>
        public static void OpenScene(string targetScene, bool asyncLoading = false)
        {
            ShowLoader();
            Instance.StartCoroutine(SceneLoader.LoadingProcess(targetScene, Instance.SceneLoadedHandler, asyncLoading));
        }
        public static void OpenScene(MinigameObject minigameObject)
        {
            ShowLoader();
            Instance.StartCoroutine(SceneLoader.LoadingProcess(minigameObject.name, Instance.SceneLoadedHandler, true));
        }

        public static void ReloadScene()
        {
            Instance.StartCoroutine(Instance.ReloadSceneCoroutine());
        }
        
        public static void RestartGame()
        {
            SceneManager.LoadScene("Main_MainScene");
        }

        /// <summary>
        /// Si une scène de niveau 2 est ouverte, elle sera déchargée.
        /// </summary>
        public static void UnloadOverlayScene()
        {
            Instance._loadedOverlayScene?.UnloadScene();
            Instance._loadedOverlayScene = null;
        }


        public static void ShowLoader()
        {
            // TODO : Disable inputs when the loader is visible
            Instance._LoaderObject?.EnableComponent();
        }

        public static void ShowBlackScreen()
        {
            Instance._BlackScreen?.EnableComponent();
        }

        public static void HideLoader()
        {
            Instance.onHideLoader?.Invoke();
            Instance._LoaderObject?.DisableComponent();
            Instance._BlackScreen?.DisableComponent();
        }
        #endregion
        #region COROUTINES----------------------------------------------------------------------


        IEnumerator ReloadSceneCoroutine()
        {
            string targetScene = Instance._loadedScene.SceneName;
            bool asyncLoading = Instance._loadedScene.AsyncLoading;
            ShowBlackScreen();
            ShowLoader();

            //minimal delay to let the black screen appear
            yield return new WaitForSeconds(0.5f);

            bool sceneUnloaded = false;

            //Action<string> sceneUnloaded = null;
            //sceneUnloaded = (string sceneName) =>
            //{
            //    if (sceneUnloadedCalled) 
            //        return; // Éviter double appel

            //    sceneUnloadedCalled = true;

            //    Instance._loadedScene.SceneUnloaded -= sceneUnloaded;
            //    Instance._loadedScene = null;
            //    OpenScene(sceneName);
            //};

            //Instance._loadedScene.SceneUnloaded += sceneUnloaded;
            //Instance._loadedScene?.UnloadScene(sceneUnloaded);
            Instance._loadedScene?.UnloadScene((string _) => sceneUnloaded = true);

            // Timeout de sécurité : attendre jusqu’à 5 secondes
            float timeout = 5f;
            float elapsed = 0f;
            while (!sceneUnloaded && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (!sceneUnloaded)
            {
                Debug.LogError("Timeout atteint : événement SceneUnloaded non reçu. Forçage de l'action.");
            }

            Instance._loadedScene = null;
            OpenScene(targetScene, asyncLoading);
        }

        #endregion
    }
}