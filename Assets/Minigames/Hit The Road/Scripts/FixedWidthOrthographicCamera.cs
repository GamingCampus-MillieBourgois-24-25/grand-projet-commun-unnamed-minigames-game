using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class FixedWidthOrthographicCamera : MonoBehaviour
{
    [SerializeField] private float targetWidth = 16f; // Largeur souhaitée en unités Unity
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
            Debug.LogWarning("La caméra doit être en mode orthographique !");
            return;
        }

        // Calculer le ratio d'aspect actuel de l'écran
        float aspectRatio = (float)Screen.width / Screen.height;

        // La taille orthographique représente la moitié de la hauteur
        // Largeur = 2 * orthographicSize * aspectRatio
        // Pour une largeur fixe : orthographicSize = targetWidth / (2 * aspectRatio)
        float orthographicSize = targetWidth / (2f * aspectRatio);

        cam.orthographicSize = orthographicSize;
    }

    // Optionnel : gérer les changements de résolution en jeu
    private void Update()
    {
        AdjustCamera();
    }
}