using Assets.Code.GLOBAL;
using UnityEngine;

public class Options_Back : MonoBehaviour
{
    private Menu_OptionsButton _menuOptionsButton;

    private void Start()
    {
        _menuOptionsButton = FindObjectOfType<Menu_OptionsButton>();
    }
    public void CloseScene()
    {
        GlobalSceneController.UnloadOverlayScene();
    }
}