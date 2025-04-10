using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class Menu_OptionsButton : MonoBehaviour
{
    public bool optionOpen;

    private void Start()
    {
        optionOpen = false;
    }

    public void OptionsLoadScene()
    {
        if (optionOpen == false)
        {
            GlobalSceneController.OpenScene(GameSettings.SettingsScene.name);
            optionOpen = true;
        }
    }
}
