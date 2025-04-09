using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_OptionsButton : MonoBehaviour
{
    public AudioClip soundButton;
    private AudioSource _audioSource;
    public bool optionOpen;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        optionOpen = false;
    }

    public void OptionsLoadScene()
    {
        if (optionOpen == false)
        {
            _audioSource.PlayOneShot(soundButton);
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            optionOpen = true;
        }
    }
}
