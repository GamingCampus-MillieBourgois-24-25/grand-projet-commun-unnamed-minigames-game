using Axoloop.Global;

public class ReviveSceneManager : BaseSceneManager<ReviveSceneManager>, ISceneManager
{
    public override string SceneName { get => "MAIN_Revive"; }

    public override SceneLevel SceneLevel { get => SceneLevel.Level2; }


    protected override void DisableScene()
    {
        // désactiver les inputs et toute action potentielle future

    }

    protected override void PlayUnloadTransition()
    {
        // jouer une animation de transition avant de décharger la scène
    }


}