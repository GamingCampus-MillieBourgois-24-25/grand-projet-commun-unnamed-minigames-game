using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopLoadScene : MonoBehaviour
{
    public AudioClip soundButton;
    private AudioSource _audioSource;
    public bool shopOpen;

    private void Start()
    {
        //_audioSource = gameObject.AddComponent<AudioSource>();
        shopOpen = false;
    }

    public void LoadShopScene()
    {
        if (shopOpen == false)
        {
            //_audioSource.PlayOneShot(soundButton);
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
            shopOpen = true;
        }
    }
}
