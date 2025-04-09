using Axoloop.Global;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : SingletonMB<MiniGameManager>
{
    public Minigame[] minigames;  // Liste des mini-jeux
    private int currentMiniGameIndex = 0;

    public void LoadNextMinigame()
    {
        if (currentMiniGameIndex < minigames.Length)
        {
            Minigame nextGame = minigames[currentMiniGameIndex];
            Debug.Log($"Chargement du mini-jeu : {nextGame.minigameName}");
            SceneManager.LoadScene(nextGame.sceneName);
            currentMiniGameIndex++;
        }
        else
        {
            Debug.Log("Tous les mini-jeux ont été joués !");
        }
    }
}
