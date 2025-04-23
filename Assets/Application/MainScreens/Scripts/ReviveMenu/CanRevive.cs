using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanRevive : MonoBehaviour
{
    [SerializeField] private Button ReviveButtonWithAD, ReviveButtonWithTicket;
    [SerializeField] private TextMeshProUGUI ticketText;

    private void Start()
    {
        CanPlayerRevive();
        ticketText.text += TryRevive.Instance.GetTicket().ToString();
    }

    private void CanPlayerRevive()
    {
        if (TryRevive.Instance.GetTicket() == 0)
        {
            ReviveButtonWithTicket.interactable = false;
        }
    }
}
