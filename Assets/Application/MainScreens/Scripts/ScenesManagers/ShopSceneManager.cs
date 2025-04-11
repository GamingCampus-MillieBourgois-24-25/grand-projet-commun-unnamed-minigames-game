using Axoloop.Global;
using Axoloop.Scripts.Global;
using System;

public class ShopSceneManager : SingletonMB<ShopSceneManager>, ISceneManager
{
    public string SceneName { get => "MAIN_Boutique"; }

    public SceneLevel SceneLevel { get => SceneLevel.Level2; }

    public Action<string> SceneLoaded { get; set; }

    public Action<string> SceneUnloaded { get; set; }



    void DisableScene()
    {
        // d�sactiver les inputs et toute action potentielle future
    }

    void PlayUnloadTransition()
    {
        // jouer une animation de transition avant de d�charger la sc�ne
    }


    /// <summary>
    /// Asynchronously load the scene and fire the SceneReady event when the scene is loaded
    /// </summary>
    public void LoadScene(Action<string> callBack)
    {
        SceneLoaded = new(callBack);
        StartCoroutine(SceneLoader.LoadingProcess(SceneName, SceneLoaded));
    }

    /// <summary>
    /// Asynchronously unloads the scene and fire the SceneUnloaded event when the scene is unloaded
    /// </summary>
    public void UnloadScene()
    {
        DisableScene();
        PlayUnloadTransition();
        StartCoroutine(SceneLoader.UnloadingProcess(SceneName, SceneUnloaded));
    }

}