using Axoloop.Global;

public class SettingsSceneManager : BaseSceneManager<SettingsSceneManager>, ISceneManager
{
    public override string SceneName { get => "MAIN_Parametres"; }

    public override SceneLevel SceneLevel { get => SceneLevel.Level2; }

    public override bool AsyncLoading { get => false; }

    protected override void DisableScene()
    {
        // d�sactiver les inputs et toute action potentielle future
    
    }

    protected override void PlayUnloadTransition()
    {
        // jouer une animation de transition avant de d�charger la sc�ne
    }


}
