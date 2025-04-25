using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGameManager : SingletonMB<MiniGameManager>
{
    [SerializeField] public Minigame[] minigames;
    private List<Minigame> unlockedMinigames = new List<Minigame>(); 
    [SerializeField] private CalculScoreAndCombo _calculScoreAndCombo;

    private void Start()
    {
        UnlockMinigames();
    }

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

    public void UnlockMinigames()
    {
        // Récupère le score actuel du joueur
        int playerScore = ScoreManager.Instance.GetTotalScore();

        // Efface la liste des mini-jeux débloqués
        unlockedMinigames.Clear();

        // Parcourt tous les mini-jeux pour les débloquer si leur score est inférieur ou égal au score du joueur
        foreach (var minigame in minigames)
        {
            if (minigame != null && minigame.scoreToUnlock <= playerScore)
            {
                unlockedMinigames.Add(minigame);
                Debug.Log($"Mini-jeu débloqué : {minigame.minigameName}");
            }
        }

        // Vérifie si des mini-jeux ont été débloqués
        if (unlockedMinigames.Count == 0)
        {
            Debug.Log("Aucun mini-jeu débloqué !");
        }
        else
        {
            Debug.Log($"Nombre de mini-jeux débloqués : {unlockedMinigames.Count}");
        }
    }
    
    public void LoadNextMinigame()
    {
        UnlockMinigames();
        
        Debug.Log("Mini-jeux valides disponibles :");

        for (int i = 0; i < unlockedMinigames.Count; i++)
        {
            var mg = unlockedMinigames[i];
            Debug.Log($"[{i}] Nom: {mg.minigameName}, Score pour débloquer: {mg.scoreToUnlock}");
        }
        
        if (unlockedMinigames.Count > 0)
        {
            
            Minigame nextGame = unlockedMinigames[Random.Range(0, unlockedMinigames.Count)];

            Debug.Log($"Chargement du mini-jeu : {nextGame.minigameName}");

            GlobalSceneController.OpenScene(nextGame.sceneName);
        }
        else
        {
            Debug.Log("Aucun mini-jeu débloqué n’est disponible !");
        }
    }
}


