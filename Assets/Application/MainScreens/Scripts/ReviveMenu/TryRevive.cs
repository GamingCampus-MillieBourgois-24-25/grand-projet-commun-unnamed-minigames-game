using Assets.Code.GLOBAL;
using UnityEngine;
using Axoloop.Global;

public class TryRevive : SingletonMB<TryRevive>
{
    [SerializeField] private int ticket;

    protected override void Awake()
    {
        base.Awake();
        if (ticket > -1)
        {
            PlayerPrefs.SetInt("Ticket", ticket);
        }
        ticket = PlayerPrefs.GetInt("Ticket");
    }

    public void ReviveWithTicket()
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

    public int GetTicket()
    {
        return ticket;
    }
}
