using Axoloop.Global;
using Axoloop.Scripts.Global;
using System;

public class HitTheRoadSceneManager : BaseSceneManager<HitTheRoadSceneManager>, ISceneManager
{
    public override string SceneName { get => "MAIN Hit the road"; }

    public override SceneLevel SceneLevel { get => SceneLevel.Level1; }

    protected override void DisableScene()
    {
        // d�sactiver les inputs et toute action potentielle future
    }

    protected override void PlayUnloadTransition()
    {
        // jouer une animation de transition avant de d�charger la sc�ne
    }

}