using Axoloop.Global;
using Axoloop.Scripts.Global;
using System;

public class SettingsSceneManager : BaseSceneManager<SettingsSceneManager>, ISceneManager
{
    public override string SceneName { get => "MAIN_Paramètres"; }

    public override SceneLevel SceneLevel { get => SceneLevel.Level2; }

    public Action<string> SceneLoaded { get; set; }

    public Action<string> SceneUnloaded { get; set; }

    protected override void DisableScene()
    {
        // désactiver les inputs et toute action potentielle future
    
    }

    protected override void PlayUnloadTransition()
    {
        // jouer une animation de transition avant de décharger la scène
    }


}
