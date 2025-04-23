using System.Collections;
using Assets.Code.GLOBAL;
using Axoloop.Global;
using UnityEngine;

public class VoxelGameManager : MonoBehaviour
{
    public static VoxelGameManager Instance;
    public GameObject victoryPanel; // Panneau de victoire
    public GameObject defeatPanel;  // Panneau de défaite

    private bool hasWon; // Vérifie si le joueur a gagné

    void Awake()
    {
        Instance = this;
        // Assurez-vous que les deux panneaux sont désactivés au début
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
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
        hasWon = won;

        if (won)
        {
            Debug.Log("Victoire !");
            defeatPanel.SetActive(false);
            victoryPanel.SetActive(true);

            RivalBike rival = FindObjectOfType<RivalBike>();
            if (rival != null)
            {
                rival.ExplodeAndEject();
            }
        }
        else
        {
            Debug.Log("Défaite !");
            victoryPanel.SetActive(false);
            defeatPanel.SetActive(true);
        }
        
        MiniGameManager.Instance.MiniGameFinished(hasWon);
    }
}


