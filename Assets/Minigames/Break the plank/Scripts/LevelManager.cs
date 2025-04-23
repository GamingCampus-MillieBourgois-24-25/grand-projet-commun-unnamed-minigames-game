using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("R�f�rences")]
    public RectTransform safeZone; // R�f�rence � la Safe Zone

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
    /// Met � jour la position et la taille de la Safe Zone.
    /// </summary>
    public void UpdateSafeZone()
    {
        if (safeZone == null)
        {
            Debug.LogError("Safe Zone non assign�e dans l'inspecteur !");
            return;
        }

        // G�n�re une nouvelle position X al�atoire
        float newX = Random.Range(minSafeZoneX, maxSafeZoneX);

        // G�n�re une nouvelle largeur al�atoire
        float newWidth = Random.Range(minSafeZoneWidth, maxSafeZoneWidth);

        // Applique la nouvelle position et taille
        safeZone.anchoredPosition = new Vector2(newX, safeZone.anchoredPosition.y);
        safeZone.sizeDelta = new Vector2(newWidth, safeZone.sizeDelta.y);

        Debug.Log($"SafeZone mise � jour | Position X: {newX}, Largeur: {newWidth}");
    }
}
