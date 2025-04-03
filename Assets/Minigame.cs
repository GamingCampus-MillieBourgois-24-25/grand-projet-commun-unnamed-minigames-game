using UnityEngine;

[CreateAssetMenu(fileName = "NewMinigame", menuName = "Minigame System/Minigame")]
public class Minigame : ScriptableObject
{
    public string minigameName;  // Nom du mini-jeu
    public string sceneName;     // Nom de la sc�ne � charger
    public Sprite minigameIcon;  // Ic�ne du mini-jeu
    public int scoreToUnlock;    // Score n�cessaire pour d�bloquer ce mini-jeu
}
