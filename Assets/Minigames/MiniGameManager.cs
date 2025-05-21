using System;
using System.Collections;
using System.Collections.Generic;
using Assets._Common.Scripts;
using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGameManager : SingletonMB<MiniGameManager>
{
    [SerializeField] public MinigameObject[] minigames;
    public List<MinigameObject> MiniGameUnlocked = new List<MinigameObject>();
    [SerializeField] private CalculScoreAndCombo _calculScoreAndCombo;
    [SerializeField] AudioClip victoryClip;
    [SerializeField] AudioClip defeatClip;

    public void PlayEndSound(bool victory)
    {
        if (victory)
        {
            GlobalAudioManager.Instance.PlaySound(victoryClip);
        }
        else
        {

            GlobalAudioManager.Instance.PlaySound(defeatClip);

        }
    }

    public void MiniGameFinished(bool victory)
    {
        if (victory)
        {
            _calculScoreAndCombo.OnMiniGameWon();
            GlobalSceneController.OpenScene(GameSettings.TransitionScene);
            StartCoroutine(DelayToStartMiniGame());
            
        }

        if (!victory)
        {
            //DisplayMiniGameIcons.Instance.UpdateIcons();
            GlobalSceneController.OpenScene(GameSettings.ReviveScene);
        }
    }
    //private IEnumerator DelayAfterLose()
    //{
    //    DisplayMiniGameIcons.Instance.UpdateIcons();
    //    GlobalSceneController.OpenScene(GameSettings.ReviveScene);
    //    yield return new WaitForSeconds(2f);
    //}
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
            MinigameObject nextGame = MiniGameUnlocked[Random.Range(0, MiniGameUnlocked.Count)];
            
            GlobalSceneController.OpenScene(nextGame.sceneName);
        }
        else
        {
            Debug.Log("Aucun mini-jeu débloqué n’est disponible !");
        }
    }
}


