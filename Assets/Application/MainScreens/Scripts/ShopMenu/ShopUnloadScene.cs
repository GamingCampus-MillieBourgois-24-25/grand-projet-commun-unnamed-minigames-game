using Assets.Code.GLOBAL;
using UnityEngine;

public class ShopUnloadScene : MonoBehaviour
{
    [SerializeField] private ShopLoadScene shopLoadSceneButton;

    private void Start()
    {
        shopLoadSceneButton = FindObjectOfType<ShopLoadScene>();
    }
    public void CloseScene()
    {
        GlobalSceneController.UnloadOverlayScene();
        shopLoadSceneButton.shopOpen = false;
    }
}
