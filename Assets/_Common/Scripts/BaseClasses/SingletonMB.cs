using UnityEngine;


namespace Axoloop.Global
{
    /// <summary>
    /// Classe permetant de faire très simplement un Singleton
    /// </summary>
    /// <typeparam name="T">Utiliser le même type que le singleton</typeparam>
    public class SingletonMB<T> : MonoBehaviour where T : SingletonMB<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.Log("Destroying duplicate singleton instance of " + typeof(T).Name + " in " + gameObject.name);
                Destroy(gameObject);
            }
            else
            {
                Instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
