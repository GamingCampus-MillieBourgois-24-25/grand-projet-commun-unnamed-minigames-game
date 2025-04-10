using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class ShopLoadScene : MonoBehaviour
{
    public bool shopOpen;

    private void Start()
    {
        shopOpen = false;
    }

    public void LoadShopScene()
    {

        GlobalSceneController.OpenScene(GameSettings.ShopScene.name);


    }
}
