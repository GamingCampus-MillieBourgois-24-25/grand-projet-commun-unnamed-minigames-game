
using UnityEngine;

public class VoxelGameManager : MonoBehaviour
{
    public static VoxelGameManager Instance;
    public GameObject victoryPanel; // Panneau de victoire
    public GameObject defeatPanel;  // Panneau de d�faite

    private bool hasWon = false; // V�rifie si le joueur a gagn�

    void Awake()
    {
        Instance = this;
        // Assurez-vous que les deux panneaux sont d�sactiv�s au d�but
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    // Appel� lorsque le joueur gagne
    public void PlayerWins()
    {
        if (hasWon) return; // Si la victoire a d�j� �t� atteinte, rien ne se passe
        Debug.Log("Victoire !");
        hasWon = true; // Marque que le joueur a gagn�

        // D�sactive le panneau de d�faite si la victoire est d�clench�e
        defeatPanel.SetActive(false);
        // Affiche le panneau de victoire
        victoryPanel.SetActive(true);

        // D�clenche l'explosion et l'�jection du RivalBike
        RivalBike rival = FindObjectOfType<RivalBike>();
        if (rival != null)
        {
            rival.ExplodeAndEject();
        }
    }

    // Appel� lorsque le joueur �choue
    public void PlayerFails()
    {
        if (hasWon) return; // Si le joueur a d�j� gagn�, ignore la d�faite
        Debug.Log("D�faite !");
        // D�sactive le panneau de victoire si la d�faite est d�clench�e
        victoryPanel.SetActive(false);
        // Affiche le panneau de d�faite
        defeatPanel.SetActive(true);
    }
}


