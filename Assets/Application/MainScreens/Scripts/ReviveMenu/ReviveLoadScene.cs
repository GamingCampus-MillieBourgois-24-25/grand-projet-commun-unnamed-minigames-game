using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class ReviveLoadScene : MonoBehaviour
{
    public void LoadScene()
    {
        GlobalSceneController.OpenScene(GameSettings.ReviveScene);
    }
}
