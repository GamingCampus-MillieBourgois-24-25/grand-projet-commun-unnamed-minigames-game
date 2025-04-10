using UnityEngine;
using Assets.Code.GLOBAL;

public class Options_Back : MonoBehaviour
{
    [SerializeField] private Menu_OptionsButton _menuOptionsButton;

    private void Start()
    {
        _menuOptionsButton = FindObjectOfType<Menu_OptionsButton>();
    }
    public void CloseScene()
    {
        GlobalSceneController.UnloadOverlayScene();
        _menuOptionsButton.optionOpen = false;
    }
}
