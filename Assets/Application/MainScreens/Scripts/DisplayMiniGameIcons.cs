using UnityEngine;
using UnityEngine.UI;

public class DisplayMiniGameIcons : MonoBehaviour
{
    public Image[] spritesMiniGames;
    public Sprite lockIcons, workIcons;

    private int Score = ScoreManager.Instance.GetTotalScore();

    private void UpdateIcons()
    {
        for (int i = 0; i < MiniGameManager.Instance.minigames.Length - 1; i++)
        {
            if (MiniGameManager.Instance.minigames[i].scoreToUnlock <= Score)
            {
                spritesMiniGames[i].sprite = MiniGameManager.Instance.minigames[i].minigameIcon;
            }
            
            spritesMiniGames[i].sprite = lockIcons;
            
            if (MiniGameManager.Instance.minigames[i] == null) spritesMiniGames[i].sprite = workIcons;
            
        }
    }
}
