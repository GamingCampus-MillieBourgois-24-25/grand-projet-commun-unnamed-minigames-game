using Axoloop.Global;
using Axoloop.Scripts.Global;
using System;

public class ShopSceneManager : BaseSceneManager<ShopSceneManager>, ISceneManager
{
    public override string SceneName { get => "MAIN_Boutique"; }

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