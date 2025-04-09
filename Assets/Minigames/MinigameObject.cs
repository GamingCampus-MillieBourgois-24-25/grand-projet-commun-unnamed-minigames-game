using UnityEngine;

[CreateAssetMenu(fileName = "NewMinigame", menuName = "Minigame System/Minigame")]
public class Minigame : ScriptableObject
{
    public string minigameName;  // Nom du mini-jeu
    public string sceneName;     // Nom de la scène à charger
    public Sprite minigameIcon;  // Icône du mini-jeu
    public int scoreToUnlock;    // Score nécessaire pour débloquer ce mini-jeu
}
