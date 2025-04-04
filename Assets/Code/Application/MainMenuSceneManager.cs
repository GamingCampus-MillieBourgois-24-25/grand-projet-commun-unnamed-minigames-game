using Axoloop.Global;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Application
{
    public class MainMenuSceneManager : SingletonMB<MainMenuSceneManager>, ISceneManager
    {

        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        public string SceneName { get => "MAIN_MainMenu"; }

        public SceneLevel SceneLevel { get => SceneLevel.Level1; }

        public Action<Scene> SceneReady { get; set; }

        public Action SceneUnloaded { get; set; }


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

        public void LoadScene()
        {
            StartCoroutine(LoadingProcess());
        }

        public void UnloadScene()
        {
            DisableScene();
            PlayUnloadTransition();
            StartCoroutine(UnloadingProcess());
        }



        #endregion
        #region COROUTINES----------------------------------------------------------------------

        IEnumerator LoadingProcess()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }

            SceneReady?.Invoke(SceneManager.GetSceneByName(SceneName));
        }

        IEnumerator UnloadingProcess()
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneName);
            asyncUnload.allowSceneActivation = false;
            while (!asyncUnload.isDone)
            {
                if (asyncUnload.progress >= 0.9f)
                {
                    asyncUnload.allowSceneActivation = true;
                }
                yield return null;
            }
            SceneUnloaded?.Invoke();
        }

        #endregion

    }
}