using Assets.Code.GLOBAL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
