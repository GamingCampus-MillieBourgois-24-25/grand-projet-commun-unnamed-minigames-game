using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("R�f�rences")]
    public RectTransform safeZone; // R�f�rence � la Safe Zone
    public RectTransform pointer; // R�f�rence au pointeur
    public Text startText; // Texte "Start"

    [Header("Safe Zone - Position et Taille")]
    public float minSafeZoneX = -200f; // Position X minimale
    public float maxSafeZoneX = 200f; // Position X maximale
    public float minSafeZoneWidth = 100f; // Largeur minimale
    public float maxSafeZoneWidth = 250f; // Largeur maximale

    private Vector2 safeZoneInitialPosition;
    private Vector2 pointerInitialPosition;

    private bool isPointerAnimationStarted = false; // Protection contre les appels multiples

    void Start()
    {
        // Sauvegarde des positions initiales
        if (safeZone != null)
        {
            safeZoneInitialPosition = safeZone.anchoredPosition;
        }
        if (pointer != null)
        {
            pointerInitialPosition = pointer.anchoredPosition;

            // Initialisez la position et l'opacit� du pointeur
            pointer.anchoredPosition = new Vector2(pointerInitialPosition.x, -Screen.height);
            CanvasGroup pointerCanvasGroup = pointer.GetComponent<CanvasGroup>();
            if (pointerCanvasGroup == null)
            {
                pointerCanvasGroup = pointer.gameObject.AddComponent<CanvasGroup>();
            }
            pointerCanvasGroup.alpha = 0f; // Rendre le pointeur invisible au d�but
        }

        // Initialisation de la Safe Zone
        UpdateSafeZone();

        // Lance l'animation d'apparition de la Safe Zone et du pointeur
        StartCoroutine(AnimateElements());
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

    /// <summary>
    /// Anime l'apparition de la Safe Zone et du pointeur, puis fait dispara�tre le texte "Start".
    /// </summary>
    private IEnumerator AnimateElements()
    {
        // Animation d'apparition de la Safe Zone
        if (safeZone != null)
        {
            yield return StartCoroutine(AnimateAppearance(safeZone, safeZoneInitialPosition, 0.8f));
        }

        // Animation d'apparition du pointeur (prot�g�e contre les appels multiples)
        if (pointer != null && !isPointerAnimationStarted)
        {
            isPointerAnimationStarted = true; // Marque l'animation comme d�marr�e
            yield return StartCoroutine(AnimateAppearance(pointer, pointerInitialPosition, 0.7f));
        }

        // Disparition du texte "Start"
        if (startText != null)
        {
            yield return new WaitForSeconds(0.8f); // Attente avant de faire dispara�tre le texte
            startText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Anime l'apparition d'un �l�ment UI depuis le bas de l'�cran avec un l�ger rebond.
    /// </summary>
    /// <param name="uiElement">Le RectTransform de l'�l�ment UI.</param>
    /// <param name="targetPosition">La position finale de l'�l�ment UI.</param>
    /// <param name="duration">La dur�e de l'animation.</param>
    /// <returns></returns>
    private IEnumerator AnimateAppearance(RectTransform uiElement, Vector2 targetPosition, float duration)
    {
        if (uiElement == null)
        {
            Debug.LogError("L'�l�ment UI � animer est null !");
            yield break;
        }

        // Ajoute un CanvasGroup pour g�rer l'opacit�
        CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.Log("Ajout d'un CanvasGroup � l'�l�ment UI.");
            canvasGroup = uiElement.gameObject.AddComponent<CanvasGroup>();
        }

        // Initialise l'opacit� et la position de d�part
        canvasGroup.alpha = 0f;
        Vector2 startPosition = new Vector2(targetPosition.x, -Screen.height); // Position en bas de l'�cran
        uiElement.anchoredPosition = startPosition;

        float elapsedTime = 0f;

        // Animation d'apparition
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolation de la position
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
            uiElement.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            // Interpolation de l'opacit�
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // Assure que la position finale et l'opacit� sont correctes
        uiElement.anchoredPosition = targetPosition;
        canvasGroup.alpha = 1f;

        // Ajoute un l�ger rebond
        yield return StartCoroutine(BounceEffect(uiElement));
    }

    /// <summary>
    /// Ajoute un effet de rebond � un �l�ment UI.
    /// </summary>
    /// <param name="uiElement">Le RectTransform de l'�l�ment UI.</param>
    /// <returns></returns>
    private IEnumerator BounceEffect(RectTransform uiElement)
    {
        float bounceDuration = 0.2f;
        float bounceHeight = 30f; // Hauteur du rebond
        Vector2 originalPosition = uiElement.anchoredPosition;

        float elapsedTime = 0f;

        // Monte l�g�rement l'�l�ment
        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / bounceDuration);
            uiElement.anchoredPosition = Vector2.Lerp(originalPosition, originalPosition + new Vector2(0, bounceHeight), t);
            yield return null;
        }

        elapsedTime = 0f;

        // Redescend l'�l�ment � sa position originale
        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / bounceDuration);
            uiElement.anchoredPosition = Vector2.Lerp(originalPosition + new Vector2(0, bounceHeight), originalPosition, t);
            yield return null;
        }

        uiElement.anchoredPosition = originalPosition; // Assure que la position finale est correcte
    }
}
