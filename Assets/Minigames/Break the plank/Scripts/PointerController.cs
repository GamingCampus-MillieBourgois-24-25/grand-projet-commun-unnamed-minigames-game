using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PointerController : MonoBehaviour
{
    [Header("Références")]
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public Text startText;
    public Text victoryText;
    public Text loseText; // Référence au texte "Lose"

    [Header("Images d'États")]
    public GameObject axoVictory; // PNG pour la victoire
    public GameObject axoLose; // PNG pour la défaite
    public GameObject axoWaiting; // PNG pour l'attente du premier clic
    public GameObject axoPointerMove; // PNG pour le mouvement du pointeur
    public GameObject axoSuccess; // PNG pour une réussite

    [Header("Paramètres")]
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float speedIncrease = 10f;
    [SerializeField] private float shakeDuration = 0.2f; // Durée de la vibration
    [SerializeField] private float shakeMagnitude = 0.1f; // Intensité de la vibration

    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private bool canMove = false;
    private int successCount = 0;
    private int successNeeded = 3;
    private int failCount = 0; // Compteur d'échecs

    private Transform mainCameraTransform;
    private Vector3 originalCameraPosition;
    private Camera mainCamera;
    private Volume postProcessingVolume;
    private DepthOfField depthOfField;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;

        // Récupère la caméra principale
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
        originalCameraPosition = mainCameraTransform.position;

        // Configure le post-traitement
        postProcessingVolume = mainCamera.GetComponent<Volume>();
        if (postProcessingVolume != null)
        {
            postProcessingVolume.profile.TryGet(out depthOfField);
        }

        // Assurez-vous que le texte "Lose" est désactivé au début
        if (loseText != null)
            loseText.gameObject.SetActive(false);

        // Affiche l'image d'attente
        ShowAxoState(axoWaiting);

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        startText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        startText.gameObject.SetActive(false);
        canMove = true;
    }

    void Update()
    {
        if (!canMove) return;

        // Affiche l'image de mouvement du pointeur
        ShowAxoState(axoPointerMove);

        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
            targetPosition = pointB.position;
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
            targetPosition = pointA.position;

        if (Input.GetKeyDown(KeyCode.Space) || IsTouching())
            CheckSuccess();
    }

    bool IsTouching()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    void CheckSuccess()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            successCount++;
            moveSpeed += speedIncrease;

            // Déclenche une vibration de la caméra
            StartCoroutine(ShakeCamera());

            Debug.Log($"Succès {successCount}/{successNeeded}");

            // Affiche l'image de réussite
            ShowAxoState(axoSuccess);

            // Déclenche le zoom immédiatement après 2 réussites
            if (successCount == 2)
            {
                StartCoroutine(ZoomAndFocusEffect());
            }

            // Vérifie si le joueur a gagné
            if (successCount >= successNeeded)
            {
                StartCoroutine(ShowVictoryAndChangeGame());
            }
        }
        else
        {
            failCount++; // Incrémente le compteur d'échecs
            Debug.Log($"Échec ! Nombre d'échecs : {failCount}");

            if (failCount >= 2)
            {
                ShowLoseText();
            }
        }
    }

    void ShowLoseText()
    {
        canMove = false; // Arrête le mouvement
        ShowAxoState(axoLose); // Affiche l'image de défaite
        if (loseText != null)
        {
            loseText.gameObject.SetActive(true); // Affiche le texte "Lose"
        }
        Debug.Log("Vous avez perdu !");
    }

    IEnumerator ShakeCamera()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            mainCameraTransform.position = originalCameraPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Réinitialise la position de la caméra
        mainCameraTransform.position = originalCameraPosition;
    }

    IEnumerator ShowVictoryAndChangeGame()
    {
        canMove = false;

        // Affiche l'image de réussite (axoSuccess)
        ShowAxoState(axoSuccess);
        yield return new WaitForSeconds(2f); // Attente de 2 secondes

        // Affiche l'image de victoire (axoVictory)
        ShowAxoState(axoVictory);

        // Affiche le texte de victoire en même temps
        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(true);
        }

        // Déclenche le zoom et les effets
        yield return StartCoroutine(ZoomAndFocusEffect());

        yield return new WaitForSeconds(2f);
        LoadNextMiniGame();
    }



    IEnumerator ZoomAndFocusEffect()
    {
        float targetOrthographicSize = 5f; // Taille orthographique cible pour le zoom
        float zoomDuration = 1f; // Durée du zoom
        float elapsedTime = 0f;

        float initialOrthographicSize = mainCamera.orthographicSize;

        // Vérifie si l'effet Depth of Field est disponible
        if (depthOfField != null)
        {
            depthOfField.active = true; // Active l'effet
        }

        // Paramètres initiaux et cibles pour le flou
        float initialFocusDistance = 30f; // Distance initiale de mise au point
        float targetFocusDistance = 10f;  // Distance cible de mise au point
        float initialAperture = 5.6f;    // Ouverture initiale
        float targetAperture = 2.8f;     // Ouverture cible (plus petite = plus de flou)

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolation de la taille orthographique
            mainCamera.orthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, elapsedTime / zoomDuration);

            // Interpolation de la mise au point et de l'ouverture
            if (depthOfField != null)
            {
                depthOfField.focusDistance.value = Mathf.Lerp(initialFocusDistance, targetFocusDistance, elapsedTime / zoomDuration);
                depthOfField.aperture.value = Mathf.Lerp(initialAperture, targetAperture, elapsedTime / zoomDuration);
            }

            yield return null;
        }

        Debug.Log("Zoom et effet de mise au point terminés.");
    }

    void LoadNextMiniGame()
    {
        Debug.Log("Chargement du prochain mini-jeu...");
        // Ici, ajoute le code pour charger ton prochain mini-jeu
        // Exemple si tu veux charger une nouvelle scène :
        // SceneManager.LoadScene("MAIN Hit the road");
    }

    void ShowAxoState(GameObject activeAxo)
    {
        // Désactive toutes les images
        if (axoVictory != null) axoVictory.SetActive(false);
        if (axoLose != null) axoLose.SetActive(false);
        if (axoWaiting != null) axoWaiting.SetActive(false);
        if (axoPointerMove != null) axoPointerMove.SetActive(false);
        if (axoSuccess != null) axoSuccess.SetActive(false);

        // Active uniquement l'image correspondant à l'état actuel
        if (activeAxo != null) activeAxo.SetActive(true);
    }
}
