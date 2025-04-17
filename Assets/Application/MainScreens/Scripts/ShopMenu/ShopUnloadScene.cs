using Assets.Code.GLOBAL;
using UnityEngine;

public class ShopUnloadScene : MonoBehaviour
{
    private ShopLoadScene _shopLoadSceneButton;

    private void Start()
    {
        _shopLoadSceneButton = FindObjectOfType<ShopLoadScene>();
    }
    public void CloseScene()
    {
        GlobalSceneController.UnloadOverlayScene();
    }
}
