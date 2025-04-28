using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanRevive : MonoBehaviour
{
    [SerializeField] private Button ReviveButtonWithAD, ReviveButtonWithTicket;
    [SerializeField] private TextMeshProUGUI ticketText;

    [SerializeField] TextMeshProUGUI score, combo;

    private void Start()
    {
        CanPlayerRevive();
        ticketText.text = TryRevive.Instance.GetTicket().ToString();
        score.text = ScoreManager.Instance.GetCurrentScore().ToString();
        combo.text = CalculScoreAndCombo.Instance.GetCombo().ToString();
    }

    private void CanPlayerRevive()
    {
        if (TryRevive.Instance.GetTicket() == 0)
        {
            ReviveButtonWithTicket.interactable = false;
        }
    }
}
