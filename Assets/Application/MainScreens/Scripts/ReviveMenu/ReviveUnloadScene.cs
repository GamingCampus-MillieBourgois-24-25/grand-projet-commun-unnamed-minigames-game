using Assets.Code.GLOBAL;
using UnityEngine;

public class ReviveUnloadScene : MonoBehaviour
{
    private ReviveLoadScene _reviveLoadSceneButton;

    private void Start()
    {
        _reviveLoadSceneButton = FindObjectOfType<ReviveLoadScene>();
    }
    public void CloseScene()
    {
        GlobalSceneController.UnloadOverlayScene();
    }
}
