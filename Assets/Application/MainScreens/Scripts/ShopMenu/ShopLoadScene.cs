using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class ShopLoadScene : MonoBehaviour
{
    public void LoadShopScene()
    {
        GlobalSceneController.OpenScene(GameSettings.ShopScene);
    }
}
