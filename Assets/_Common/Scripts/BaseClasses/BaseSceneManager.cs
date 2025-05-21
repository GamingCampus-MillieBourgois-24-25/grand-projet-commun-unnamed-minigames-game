using Axoloop.Scripts.Global;
using System;

namespace Axoloop.Global
{
    
    public abstract class BaseSceneManager<T> : SingletonMB<T>, ISceneManager where T : BaseSceneManager<T>
    {
        public abstract string SceneName { get; }
        public abstract SceneLevel SceneLevel { get; }
        public abstract bool AsyncLoading { get; }

        public Action<string> SceneLoaded { get; set; }
        public Action<string> SceneUnloaded { get; set; }

        // Méthodes à implémenter dans les classes dérivées
        protected abstract void DisableScene();
        protected abstract void PlayUnloadTransition();
        protected virtual void PlayLoadTransition()
        {

        }

        // Implémentation commune de l'API
        public void LoadScene(Action<string> callBack = null)
        {
            SceneLoaded += callBack;
            StartCoroutine(SceneLoader.LoadingProcess(SceneName, SceneLoaded, AsyncLoading));
        }

        public void UnloadScene(Action<string> callBack = null)
        {
            SceneUnloaded += callBack;
            DisableScene();
            PlayUnloadTransition();
            StartCoroutine(SceneLoader.UnloadingProcess(SceneName, SceneUnloaded));
        }

    }
}
