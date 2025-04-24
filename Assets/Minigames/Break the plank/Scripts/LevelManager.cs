using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Références")]
    public RectTransform safeZone; // Référence à la Safe Zone

    [Header("Safe Zone - Position et Taille")]
    public float minSafeZoneX = -200f; // Position X minimale
    public float maxSafeZoneX = 200f; // Position X maximale
    public float minSafeZoneWidth = 100f; // Largeur minimale
    public float maxSafeZoneWidth = 250f; // Largeur maximale

    void Start()
    {
        // Initialisation de la Safe Zone
        UpdateSafeZone();
    }

    /// <summary>
    /// Met à jour la position et la taille de la Safe Zone.
    /// </summary>
    public void UpdateSafeZone()
    {
        if (safeZone == null)
        {
            Debug.LogError("Safe Zone non assignée dans l'inspecteur !");
            return;
        }

        // Génère une nouvelle position X aléatoire
        float newX = Random.Range(minSafeZoneX, maxSafeZoneX);

        // Génère une nouvelle largeur aléatoire
        float newWidth = Random.Range(minSafeZoneWidth, maxSafeZoneWidth);

        // Applique la nouvelle position et taille
        safeZone.anchoredPosition = new Vector2(newX, safeZone.anchoredPosition.y);
        safeZone.sizeDelta = new Vector2(newWidth, safeZone.sizeDelta.y);

        Debug.Log($"SafeZone mise à jour | Position X: {newX}, Largeur: {newWidth}");
    }
}
