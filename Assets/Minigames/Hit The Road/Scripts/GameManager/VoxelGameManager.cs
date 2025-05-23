using System.Collections;
using Assets.Code.GLOBAL;
using Axoloop.Global;
using AxoLoop.Minigames.HitTheRoad;
using UnityEngine;

public class VoxelGameManager : MonoBehaviour
{
    public static VoxelGameManager Instance;
    public ContinueText continueText;
    public GameObject defeatTrigger;

    public RivalBike rivalBike;

    bool gameEnd = false; // Vérifie si le jeu est terminé

    void Awake()
    {
        Instance = this;
        // Assurez-vous que les deux panneaux sont désactivés au début
    }

    // Appelé lorsque le joueur gagne
    public void PlayerWins()
    {
        EndGame(true);
    }

    // Appelé lorsque le joueur échoue
    public void PlayerFails()
    {
        EndGame(false);
    }

    public void EndGame(bool won)
    {
        if (gameEnd) return;

        gameEnd = true;

        defeatTrigger.SetActive(false);
        MiniGameManager.Instance?.PlayEndSound(won); // Joue le son de victoire ou de défaite
        PlayerBike.Instance.PlayEndAnimation(won);
        
        if (won)
        {
            Debug.Log("Victoire !");
            rivalBike.ExplodeAndEject();
            MinigameHelper.IncrementMinigamePlayed(HitTheRoadController.Instance.hitTheRoad);
        }
        else
        {
            Debug.Log("Défaite !");
        }
        continueText.Enable(won);
        // attendre input du joueur
        //MiniGameManager.Instance.MiniGameFinished(hasWon);
    }
}


