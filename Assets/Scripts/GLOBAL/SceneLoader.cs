using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Axoloop.Scripts.Global
{
    public static class SceneLoader
    {
        // Ctrl + M + O pour déplier toutes les régions

        #region API-----------------------------------------------------------------------------

        #endregion
        #region COROUTINES----------------------------------------------------------------------

        public static IEnumerator LoadingProcess(string sceneName, Action<string> onFinishedCallback)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            asyncLoad.completed += (operation) => onFinishedCallback.Invoke(sceneName);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        public static IEnumerator UnloadingProcess(string sceneName, Action<string> onFinishedCallback)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            asyncUnload.completed += (operation) => onFinishedCallback.Invoke(sceneName);
            asyncUnload.allowSceneActivation = false;

            while (!asyncUnload.isDone)
            {
                if (asyncUnload.progress >= 0.9f)
                {
                    asyncUnload.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        #endregion

    }
}