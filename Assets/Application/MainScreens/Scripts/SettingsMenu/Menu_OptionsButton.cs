using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class Menu_OptionsButton : MonoBehaviour
{
    public void OptionsLoadScene()
    {
        GlobalSceneController.OpenScene(GameSettings.SettingsScene.name);
    }
}