using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMiniGameIcons : MonoBehaviour
{
    public Image[] spritesMiniGames;
    public Sprite workIcons;

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

        for (var i = 0; i < MiniGameManager.Instance.minigames.Length; i++)
        {
            var miniGame = MiniGameManager.Instance.minigames[i];

            if (miniGame == null)
            {
                spritesMiniGames[i].sprite = workIcons;
                continue;
            }
            if (miniGame.scoreToUnlock <= _score)
            {
                spritesMiniGames[i].sprite = miniGame.minigameIcon;
            }
            else
            {
                spritesMiniGames[i].sprite = miniGame.miniGameIconLock;
            }
        }
    }
}
