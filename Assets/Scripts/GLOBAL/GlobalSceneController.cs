using Assets.Scripts.GLOBAL;
using Axoloop.Global;
using Axoloop.Global.UI;
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
        private ISceneManager _loadedBackgroundScene;
        private ISceneManager _loadedOverlayScene;
        private ISceneManager _loadingScene;

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
            OpenScene(StartScreenSceneManager.Instance);
        }

        void SceneLoadedHandler(string sceneName)
        {
            _loadingScene.SceneLoaded -= SceneLoadedHandler; // se désabonner immédiatement

            switch (_loadingScene.SceneLevel)
            {
                case SceneLevel.Level1:
                    _loadedBackgroundScene = _loadingScene;
                    break;
                case SceneLevel.Level2:
                    _loadedOverlayScene = _loadingScene;
                    break;
            }

            var newScene = _loadedScene;
            _loadedScene = null;

            HideLoader();
        }

        #endregion
        #region API-----------------------------------------------------------------------------

        /// <summary>
        /// Ouvre n'importe quelle scène. <br></br>
        /// Si la scène est de niveau 1, elle sera ouverte en arrière plan et la scène actuelle sera déchargée.<br></br>
        /// Si la scène est de niveau 2, elle sera ouverte immédiatement.
        /// </summary>
        /// <param name="targetScene"></param>
        public static void OpenScene(ISceneManager targetScene)
        {
            UnloadOverlayScene(); //Immédiatement décharger toute scène de niveau 2 potentielle
            ShowLoader();

            Instance._loadingScene = targetScene;

            targetScene.LoadScene(Instance.SceneLoadedHandler); //nullReferenceException ici

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