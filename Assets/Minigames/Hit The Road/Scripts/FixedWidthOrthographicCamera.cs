using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class FixedWidthOrthographicCamera : MonoBehaviour
{
    [SerializeField] private float targetWidth = 16f; // Largeur souhait�e en unit�s Unity
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        if (!cam.orthographic)
        {
            Debug.LogWarning("La cam�ra doit �tre en mode orthographique !");
            return;
        }

        // Calculer le ratio d'aspect actuel de l'�cran
        float aspectRatio = (float)Screen.width / Screen.height;

        // La taille orthographique repr�sente la moiti� de la hauteur
        // Largeur = 2 * orthographicSize * aspectRatio
        // Pour une largeur fixe : orthographicSize = targetWidth / (2 * aspectRatio)
        float orthographicSize = targetWidth / (2f * aspectRatio);

        cam.orthographicSize = orthographicSize;
    }

    // Optionnel : g�rer les changements de r�solution en jeu
    private void Update()
    {
        AdjustCamera();
    }
}