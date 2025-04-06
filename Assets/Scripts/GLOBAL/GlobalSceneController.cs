using Assets.Scripts.GLOBAL;
using Axoloop.Global;
using Axoloop.Global.UI;
using Axoloop.Scripts.Global;
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

        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------

        private void Start()
        {
            InitialLoading();
        }

        #endregion
        #region METHODS-------------------------------------------------------------------------

        void InitialLoading()
        {
            OpenScene(GameSettings.StartScene.name);
        }

        void SceneLoadedHandler(string sceneName)
        {
            Scene loadedScene = SceneManager.GetSceneByName(sceneName);

            ISceneManager loadedSceneManager = FindSceneManagerInstance(loadedScene);

            if(loadedSceneManager == null)
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
        /// Find the SceneManager in the scene
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

        #endregion
        #region API-----------------------------------------------------------------------------

        /// <summary>
        /// Ouvre n'importe quelle scène. <br></br>
        /// Si la scène est de niveau 1, elle sera ouverte en arrière plan et la scène actuelle sera déchargée.<br></br>
        /// Si la scène est de niveau 2, elle sera ouverte immédiatement.
        /// </summary>
        /// <param name="targetScene"></param>
        public static void OpenScene(string targetScene)
        {
            ShowLoader();
            Instance.StartCoroutine(SceneLoader.LoadingProcess(targetScene, Instance.SceneLoadedHandler));
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
            Instance._LoaderObject?.DisableComponent();
            Instance._BlackScreen?.DisableComponent();
        }
        #endregion
        #region COROUTINES----------------------------------------------------------------------




        #endregion
    }
}