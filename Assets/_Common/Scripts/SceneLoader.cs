using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Axoloop.Scripts.Global
{
    public static class SceneLoader
    {
        private static Action callback;

        // Ctrl + M + O pour déplier toutes les régions

        #region API-----------------------------------------------------------------------------

        public static void FinishLoading()
        {
            callback?.Invoke();
            callback = null;
        }

        #endregion
        #region COROUTINES----------------------------------------------------------------------

        public static IEnumerator LoadingProcess(string sceneName, Action<string> onFinishedCallback, bool asyncLoading = false)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if (asyncLoading)
            {
                // Let the scene execute some additional initialization logic in parallel and wait for it to finish by calling FinishLoading()
                callback = () => onFinishedCallback?.Invoke(sceneName);
            }
            else
            {
                // Finish the loading process as soon as the scene is loaded
                asyncLoad.completed += (operation) => onFinishedCallback.Invoke(sceneName);
            }

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

            if (onFinishedCallback != null)
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