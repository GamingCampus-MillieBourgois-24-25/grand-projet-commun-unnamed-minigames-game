using UnityEngine;
using Assets.Code.GLOBAL;
using Axoloop.Global;

public class ShopLoadScene : MonoBehaviour
{
    public AudioClip soundButton;
    private AudioSource _audioSource;
    public bool shopOpen;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        shopOpen = false;
    }

    public void LoadShopScene()
    {
        if (shopOpen == false)
        {
            _audioSource.PlayOneShot(soundButton);
            GlobalSceneController.OpenScene(GameSettings.ShopScene.name);
            shopOpen = true;
        }
    }
}
