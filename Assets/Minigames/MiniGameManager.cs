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
    public void LoadNextMinigame()
    {
        List<Minigame> validMinigames = minigames.Where(m => m != null).ToList();
        if (validMinigames.Count > 0)
        {
            Minigame nextGame = validMinigames[Random.Range(0, validMinigames.Count)];
            
            Debug.Log($"Chargement du mini-jeu : {nextGame.minigameName}");
            
            GlobalSceneController.OpenScene(nextGame.sceneName);
        }
        else
        {
            Debug.Log("Tous les mini-jeux ont �t� jou�s !");
        }
    }
}


