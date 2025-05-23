﻿using Axoloop.Global;
using Axoloop.Scripts.Global;
using System;

namespace Assets.Scripts.GLOBAL
{
    public class StartScreenSceneManager : SingletonMB<StartScreenSceneManager>, ISceneManager
    {

        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        public string SceneName { get => "MAIN_StartScreen"; }

        public SceneLevel SceneLevel { get => SceneLevel.Level1; }

        public bool AsyncLoading { get => false; }
        
        public Action<string> SceneLoaded { get; set; }

        public Action<string> SceneUnloaded { get; set; }




        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------



        #endregion
        #region METHODS-------------------------------------------------------------------------


        void DisableScene()
        {

        }

        void PlayUnloadTransition()
        {

        }



        #endregion
        #region API-----------------------------------------------------------------------------

        /// <summary>
        /// Asynchronously load the scene and fire the SceneReady event when the scene is loaded
        /// </summary>
        public void LoadScene(Action<string> callBack)
        {
            SceneLoaded = new(callBack);
            StartCoroutine(SceneLoader.LoadingProcess(SceneName, SceneLoaded, false));
        }

        /// <summary>
        /// Asynchronously unloads the scene and fire the SceneUnloaded event when the scene is unloaded
        /// </summary>
        public void UnloadScene(Action<string> callBack = null)
        {
            SceneUnloaded += callBack;
            DisableScene();
            PlayUnloadTransition();
            StartCoroutine(SceneLoader.UnloadingProcess(SceneName, SceneUnloaded));
        }




        #endregion
        #region COROUTINES----------------------------------------------------------------------

        #endregion

    }
}