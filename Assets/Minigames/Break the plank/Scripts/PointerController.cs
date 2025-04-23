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
    [Header("Références supplémentaires")]
    public LevelManager levelManager; // Référence au LevelManager

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
    private int successNeeded = 2;
    private int failCount = 0; // Compteur d'échecs

    private Transform mainCameraTransform;
    private Vector3 originalCameraPosition;
    private Camera mainCamera;
    private Volume postProcessingVolume;
    private DepthOfField depthOfField;

    void Start()
    {
        // Validation des références
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Les points A et B ne sont pas assignés !");
            return;
        }

        pointerTransform = GetComponent<RectTransform>();
        if (pointerTransform == null)
        {
            Debug.LogError("RectTransform manquant sur l'objet PointerController !");
            return;
        }

        targetPosition = pointB.position;

        // Récupère la caméra principale
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
            originalCameraPosition = mainCameraTransform.position;

            // Configure le post-traitement
            postProcessingVolume = mainCamera.GetComponent<Volume>();
            if (postProcessingVolume != null)
            {
                postProcessingVolume.profile.TryGet(out depthOfField);
            }
        }
        else
        {
            Debug.LogError("Caméra principale introuvable !");
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
        if (startText != null)
        {
            startText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            startText.gameObject.SetActive(false);
        }
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

    IEnumerator HammerEffect()
    {
        Vector3 originalPosition = pointerTransform.position; // Sauvegarde la position initiale
        Vector3 hammerPosition = originalPosition + new Vector3(0, -20f, 0); // Position légèrement en dessous

        // Descend le pointeur
        pointerTransform.position = hammerPosition;
        yield return new WaitForSeconds(0.1f); // Attente courte pour l'effet de descente

        // Remonte le pointeur à sa position initiale
        pointerTransform.position = originalPosition;
    }

    void CheckSuccess()
    {
        // Lance l'effet de marteau
        StartCoroutine(HammerEffect());

        if (safeZone == null)
        {
            Debug.LogError("Safe Zone non assignée !");
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            successCount++;
            moveSpeed += speedIncrease;

            // Déclenche une vibration de la caméra
            StartCoroutine(ShakeCamera());

            Debug.Log($"Succès {successCount}/{successNeeded}");

            // Affiche l'image de réussite
            ShowAxoState(axoSuccess);

            // Change la position de la safeZone via le LevelManager
            if (levelManager != null)
            {
                levelManager.UpdateSafeZone(); // Met à jour la Safe Zone
            }
            else
            {
                Debug.LogError("LevelManager non assigné !");
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
        if (mainCameraTransform == null) yield break;

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

        yield return new WaitForSeconds(2f);
        LoadNextMiniGame();
    }

    void LoadNextMiniGame()
    {
        Debug.Log("Chargement du prochain mini-jeu...");
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
