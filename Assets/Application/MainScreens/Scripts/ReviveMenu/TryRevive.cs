using Assets.Code.GLOBAL;
using UnityEngine;
using Axoloop.Global;

public class TryRevive : MonoBehaviour
{
    public int ticket;
    public static TryRevive Instance;

    private void Awake()
    {
        ticket = PlayerPrefs.GetInt("Ticket");
        Instance = this;
    }

    public void ReviveWithTIcket()
    {
        if (ticket <= 0)
        {
            return;
        }
        
        ticket--;
        PlayerPrefs.SetInt("Ticket", ticket);
        GlobalSceneController.ReloadScene();
    }

    public void ReviveWithAD()
    {
        GlobalSceneController.ReloadScene();
    }

    public void DontRevive()
    {
        GlobalSceneController.OpenScene(GameSettings.MainMenuScene);
    }
}
