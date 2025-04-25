using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGameManager : SingletonMB<MiniGameManager>
{
    [SerializeField] public Minigame[] minigames;
    public List<Minigame> MiniGameUnlocked = new List<Minigame>();
    [SerializeField] private CalculScoreAndCombo _calculScoreAndCombo;
    

    public void MiniGameFinished(bool victory)
    {
        if (victory)
        {
            Action  Step2 = () => StartCoroutine(DelayToStartMiniGame());
            StartCoroutine(DelayAfterWin(Step2));
            
            _calculScoreAndCombo.OnMiniGameWon();
        }

        if (!victory)
        {
            StartCoroutine(DelayAfterLose());
        }
    }
    private IEnumerator DelayAfterWin(Action callback)
    {
        yield return new WaitForSeconds(2.5f);
        GlobalSceneController.OpenScene(GameSettings.TransitionScene);
        callback.Invoke();
    }
    private IEnumerator DelayAfterLose()
    {
        yield return new WaitForSeconds(2f);
        DisplayMiniGameIcons.Instance.UpdateIcons();
        GlobalSceneController.OpenScene(GameSettings.ReviveScene);
    }
    private IEnumerator DelayToStartMiniGame()
    {
        yield return new WaitForSeconds(2.5f);
        LoadNextMinigame();
    }


    private void UnlockMinigames()
    {
        var playerScore = ScoreManager.Instance.GetTotalScore();
        
        MiniGameUnlocked.Clear();
        
        foreach (var minigame in minigames)
        {
            if (minigame != null && minigame.scoreToUnlock <= playerScore)
            {
                MiniGameUnlocked.Add(minigame);
                Debug.Log($"Mini-jeu débloqué : {minigame.minigameName}");
            }
        }
        
        if (MiniGameUnlocked.Count == 0)
        {
            Debug.Log("Aucun mini-jeu débloqué !");
        }
        else
        {
            Debug.Log($"Nombre de mini-jeux débloqués : {MiniGameUnlocked.Count}");
            
            Debug.Log("Liste des mini-jeux débloqués :");
            foreach (var minigame in MiniGameUnlocked)
            {
                Debug.Log($"- {minigame.minigameName} (Score pour débloquer : {minigame.scoreToUnlock})");
            }
        }
    }
    
    public void LoadNextMinigame()
    {
        UnlockMinigames();
        Debug.Log($"Nombre de mini-jeux débloqués disponibles : {MiniGameUnlocked.Count}");

        if (MiniGameUnlocked.Count > 0)
        {
            Minigame nextGame = MiniGameUnlocked[Random.Range(0, MiniGameUnlocked.Count)];
            
            GlobalSceneController.OpenScene(nextGame.sceneName);
        }
        else
        {
            Debug.Log("Aucun mini-jeu débloqué n’est disponible !");
        }
    }
}


