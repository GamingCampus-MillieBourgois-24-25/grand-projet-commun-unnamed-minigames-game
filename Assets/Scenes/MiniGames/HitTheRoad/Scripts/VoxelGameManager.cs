
using UnityEngine;

public class VoxelGameManager : MonoBehaviour
{
    public static VoxelGameManager Instance;
    public GameObject victoryPanel; // Panneau de victoire
    public GameObject defeatPanel;  // Panneau de défaite

    private bool hasWon = false; // Vérifie si le joueur a gagné

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
        if (hasWon) return; // Si la victoire a déjà été atteinte, rien ne se passe
        Debug.Log("Victoire !");
        hasWon = true; // Marque que le joueur a gagné

        // Désactive le panneau de défaite si la victoire est déclenchée
        defeatPanel.SetActive(false);
        // Affiche le panneau de victoire
        victoryPanel.SetActive(true);

        // Déclenche l'explosion et l'éjection du RivalBike
        RivalBike rival = FindObjectOfType<RivalBike>();
        if (rival != null)
        {
            rival.ExplodeAndEject();
        }
    }

    // Appelé lorsque le joueur échoue
    public void PlayerFails()
    {
        if (hasWon) return; // Si le joueur a déjà gagné, ignore la défaite
        Debug.Log("Défaite !");
        // Désactive le panneau de victoire si la défaite est déclenchée
        victoryPanel.SetActive(false);
        // Affiche le panneau de défaite
        defeatPanel.SetActive(true);
    }
}


