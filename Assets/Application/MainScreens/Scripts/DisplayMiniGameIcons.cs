using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMiniGameIcons : MonoBehaviour
{
    public Image[] spritesMiniGames;
    public Sprite lockIcons, workIcons;

    public static DisplayMiniGameIcons Instance;
    
    private int _score;

    private void Start()
    {
        Instance = this;
        UpdateIcons();
    }

    public void UpdateIcons()
    {
        _score = ScoreManager.Instance.GetTotalScore();
        
        for (int i = 0; i < MiniGameManager.Instance.minigames.Length; i++)
        {
            var minigame = MiniGameManager.Instance.minigames[i];

            if (minigame == null)
            {
                spritesMiniGames[i].sprite = workIcons;
            }
            else if (minigame.scoreToUnlock <= _score)
            {
                spritesMiniGames[i].sprite = minigame.minigameIcon;
            }
            else
            {
                spritesMiniGames[i].sprite = lockIcons;
            }
        }
    }
}
