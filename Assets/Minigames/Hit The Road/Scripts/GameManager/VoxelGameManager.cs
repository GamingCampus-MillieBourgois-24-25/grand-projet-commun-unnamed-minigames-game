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

    private bool hasWon; // Vérifie si le joueur a gagné

    void Awake()
    {
        Instance = this;
        // Assurez-vous que les deux panneaux sont désactivés au début
    }

    // Appelé lorsque le joueur gagne
    public void PlayerWins()
    {
        defeatTrigger.SetActive(false);
        EndGame(true);
    }

    // Appelé lorsque le joueur échoue
    public void PlayerFails()
    {
        EndGame(false);
    }

    public void EndGame(bool won)
    {
        hasWon = won;

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


