using Axoloop.Scripts.Global;
using System;

namespace Axoloop.Global
{
    
    public abstract class BaseSceneManager<T> : SingletonMB<T>, ISceneManager where T : BaseSceneManager<T>
    {
        public abstract string SceneName { get; }
        public abstract SceneLevel SceneLevel { get; }
        public Action<string> SceneLoaded { get; set; }
        public Action<string> SceneUnloaded { get; set; }

        // Méthodes à implémenter dans les classes dérivées
        protected abstract void DisableScene();
        protected abstract void PlayUnloadTransition();

        // Implémentation commune de l'API
        public void LoadScene(Action<string> callBack = null)
        {
            SceneLoaded += callBack;
            StartCoroutine(SceneLoader.LoadingProcess(SceneName, SceneLoaded));
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
