using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanRevive : MonoBehaviour
{
    public Button ReviveButtonWithAD, ReviveButtonWithTicket;
    public TextMeshProUGUI ticketText;

    private void Start()
    {
        CanPlayerRevive();
        ticketText.text += TryRevive.Instance.ticket.ToString();
    }

    public void CanPlayerRevive()
    {
        if (TryRevive.Instance.ticket == 0)
        {
            ReviveButtonWithTicket.interactable = false;
        }
    }
}
