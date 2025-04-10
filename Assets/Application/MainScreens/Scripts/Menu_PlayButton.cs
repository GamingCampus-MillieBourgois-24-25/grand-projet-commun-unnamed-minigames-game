using UnityEngine;
using Assets.Code.GLOBAL;
using Axoloop.Global;

public class Menu_PlayButton : MonoBehaviour
{
    public void PlayLoadScene()
    {
        GlobalSceneController.OpenScene(GameSettings.MainMenuScene.name);
    }
}
