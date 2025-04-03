using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Références")]
    public PointerController pointerController;
    public RectTransform safeZone;
    public Text levelText;

    [Header("Paramètres de Niveau")]
    public int currentLevel = 1;

    [Header("Vitesse du Pointer")]
    public float baseSpeed = 80f;
    public float speedIncrement = 10f;

    [Header("Safe Zone - Position et Taille")]
    public float minSafeZoneX = -200f;
    public float maxSafeZoneX = 200f;
    public float minSafeZoneWidth = 100f;
    public float maxSafeZoneWidth = 250f;

    void Start()
    {
        ApplyLevelSettings();
    }

    public void NextLevel()
    {
        currentLevel++;
        ApplyLevelSettings();
    }

    void ApplyLevelSettings()
    {
        // Modifier la position X et la largeur de la Safe Zone
        float newX = Random.Range(minSafeZoneX, maxSafeZoneX);
        float newWidth = Random.Range(minSafeZoneWidth, maxSafeZoneWidth);

        safeZone.anchoredPosition = new Vector2(newX, safeZone.anchoredPosition.y);
        safeZone.sizeDelta = new Vector2(newWidth, safeZone.sizeDelta.y);

        if (levelText != null)
            levelText.text = "Niveau " + currentLevel;

        Debug.Log($"Niveau {currentLevel} | SafeZone X: {newX} | Largeur: {newWidth}");
    }
}
