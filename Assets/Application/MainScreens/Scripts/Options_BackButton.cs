using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options_Back : MonoBehaviour
{
    [SerializeField] private Menu_OptionsButton _menuOptionsButton;

    private void Start()
    {
        _menuOptionsButton = FindObjectOfType<Menu_OptionsButton>();
    }
    public void CloseScene()
    {
        SceneManager.UnloadSceneAsync(1);
        _menuOptionsButton.optionOpen = false;
    }
}
