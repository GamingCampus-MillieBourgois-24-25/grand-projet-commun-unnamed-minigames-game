using UnityEngine;
using Assets.Code.GLOBAL;
using Axoloop.Global;

public class ShopLoadScene : MonoBehaviour
{
    public bool shopOpen;

    private void Start()
    {
        shopOpen = false;
    }

    public void LoadShopScene()
    {
        if (shopOpen == false)
        {
            GlobalSceneController.OpenScene(GameSettings.ShopScene.name);
            shopOpen = true;
        }
    }
}
