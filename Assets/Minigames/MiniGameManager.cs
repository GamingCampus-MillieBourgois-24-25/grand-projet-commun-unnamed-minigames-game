using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class MiniGameManager : SingletonMB<MiniGameManager>
{
    public Minigame[] minigames;  // Liste des mini-jeux
    private int currentMiniGameIndex;

    public void MiniGameWinned(bool victory)
    {
        
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

    public void ClearScene()
    {
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            if (!obj.CompareTag("Persistent") && obj.name != "DontDestroyOnLoad")
            {
                GameObject.Destroy(obj);
            }
        }
    }
}


