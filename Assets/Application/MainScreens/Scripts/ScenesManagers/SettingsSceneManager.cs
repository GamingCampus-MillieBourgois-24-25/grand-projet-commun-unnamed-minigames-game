using Axoloop.Global;
using Axoloop.Scripts.Global;
using System;

public class SettingsSceneManager : BaseSceneManager<SettingsSceneManager>, ISceneManager
{
    public override string SceneName { get => "MAIN_Param�tres"; }

    public override SceneLevel SceneLevel { get => SceneLevel.Level2; }

    public Action<string> SceneLoaded { get; set; }

    public Action<string> SceneUnloaded { get; set; }

    protected override void DisableScene()
    {
        // d�sactiver les inputs et toute action potentielle future
    
    }

    protected override void PlayUnloadTransition()
    {
        // jouer une animation de transition avant de d�charger la sc�ne
    }


}
