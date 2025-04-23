using System;
using System.Collections;
using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class MiniGameManager : SingletonMB<MiniGameManager>
{
    public Minigame[] minigames;  // Liste des mini-jeux
    private int currentMiniGameIndex;
    public bool isWin;
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
        GlobalSceneController.OpenScene(GameSettings.TransitionScene.name);
        callback.Invoke();
    }

    private IEnumerator DelayAfterLose()
    {
        yield return new WaitForSeconds(2f);
        GlobalSceneController.OpenScene(GameSettings.MainMenuScene);
    }

    private IEnumerator DelayToStartMiniGame()
    {
        yield return new WaitForSeconds(2.5f);
        LoadNextMinigame();
    }
    public void LoadNextMinigame()
    {
        currentMiniGameIndex = 0;
        if (currentMiniGameIndex < minigames.Length)
        {
            Minigame nextGame = minigames[currentMiniGameIndex];
            Debug.Log($"Chargement du mini-jeu : {nextGame.minigameName}");
            GlobalSceneController.OpenScene(nextGame.sceneName);
            currentMiniGameIndex++;
        }
        else
        {
            Debug.Log("Tous les mini-jeux ont �t� jou�s !");
        }
    }
}


