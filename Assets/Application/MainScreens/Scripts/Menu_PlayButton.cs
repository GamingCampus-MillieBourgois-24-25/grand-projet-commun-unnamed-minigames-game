using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class Menu_PlayButton : MonoBehaviour
{
    public void PlayLoadScene()
    {
        GlobalSceneController.OpenScene(GameSettings.MainMenuScene);
    }
}
