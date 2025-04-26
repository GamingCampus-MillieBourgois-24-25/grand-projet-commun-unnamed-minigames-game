using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PointerController : MonoBehaviour, IMinigameController
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

    [Header("Plank Sprites")]
    public GameObject plank_full; // Plank intact
    public GameObject plank_break_middle; // Plank cassé au milieu
    public GameObject plank_break_right; // Plank cassé à droite
    public GameObject plank_break_left; // Plank cassé à gauche

    [Header("Paramètres")]
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float speedIncrease = 10f;
    [SerializeField] private float shakeDuration = 0.15f; // Durée de la vibration
    [SerializeField] private float shakeMagnitude = 0.08f; // Intensité de la vibration

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
    bool moveArrow = true;
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

        // Affiche le plank intact au début
        ResetPlankState();

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

        if(moveArrow) pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
            targetPosition = pointB.position;
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
            targetPosition = pointA.position;

        if (Input.GetKeyDown(KeyCode.Space) || IsTouching())
            PlayAction();
    }

    bool IsTouching()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    IEnumerator HammerEffect(System.Action callback)
    {
        moveArrow = false;
        Vector3 originalPosition = pointerTransform.position; // Sauvegarde la position initiale
        Vector3 recoilPosition = originalPosition + new Vector3(0, 50f, 0); // Position légèrement au-dessus
        Vector3 hammerPosition = originalPosition + new Vector3(0, -50f, 0); // Position légèrement en dessous

        float duration = 0.2f; // Durée pour chaque étape (recul et descente)

        // Étape 1 : Monte légèrement pour l'effet de recul
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            pointerTransform.position = Vector3.Lerp(originalPosition, recoilPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pointerTransform.position = recoilPosition; // Assure la position finale

        // Étape 2 : Descend pour l'effet de marteau
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            pointerTransform.position = Vector3.Lerp(recoilPosition, hammerPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pointerTransform.position = hammerPosition; // Assure la position finale

        // Quand le pointeur a fini de descendre
        callback.Invoke();

        // Étape 3 : Remonte à la position initiale
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            pointerTransform.position = Vector3.Lerp(hammerPosition, originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pointerTransform.position = originalPosition; // Assure la position finale

        moveArrow = true;
    }



    void PlayAction()
    {
        // Lance l'effet de marteau
        StartCoroutine(HammerEffect(CheckSuccess));
    }

    void CheckSuccess()
    {

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

            // Affiche le plank correspondant uniquement après la 2ème frappe réussie
            if (successCount == successNeeded)
            {
                UpdatePlankState();
                StartCoroutine(FadeOutSafeZone()); // Lance la disparition de la Safe Zone
            }

            // Change la position de la safeZone via le LevelManager uniquement si le nombre de succès est inférieur à successNeeded
            if (levelManager != null && successCount < successNeeded)
            {
                levelManager.UpdateSafeZone(); // Met à jour la Safe Zone
            }
            else if (successCount >= successNeeded)
            {
                Debug.Log("La Safe Zone ne se déplace plus.");
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

    IEnumerator FadeOutSafeZone()
    {
        if (safeZone == null) yield break;

        CanvasGroup canvasGroup = safeZone.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // Ajoute un CanvasGroup si non présent
            canvasGroup = safeZone.gameObject.AddComponent<CanvasGroup>();
        }

        float duration = 0.7f; // Durée de la disparition
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration); // Réduit l'opacité
            yield return null;
        }

        canvasGroup.alpha = 0f; // Assure que l'opacité est à 0
        safeZone.gameObject.SetActive(false); // Désactive la Safe Zone après la disparition
    }

    void UpdatePlankState()
    {
        // Désactive tous les GameObjects
        plank_full.SetActive(false);
        plank_break_middle.SetActive(false);
        plank_break_right.SetActive(false);
        plank_break_left.SetActive(false);

        // Détermine la position de la Safe Zone
        float safeZoneX = safeZone.anchoredPosition.x;

        // Définir des plages précises pour chaque sprite
        if (safeZoneX < -300) // Zone très à gauche
        {
            plank_break_left.SetActive(true);
        }
        else if (safeZoneX >= -300 && safeZoneX < -100) // Zone légèrement à gauche
        {
            plank_break_left.SetActive(true);
        }
        else if (safeZoneX >= -100 && safeZoneX <= 100) // Zone centrale
        {
            plank_break_middle.SetActive(true);
        }
        else if (safeZoneX > 100 && safeZoneX <= 300) // Zone légèrement à droite
        {
            plank_break_right.SetActive(true);
        }
        else if (safeZoneX > 300) // Zone très à droite
        {
            plank_break_right.SetActive(true);
        }
    }

    void ResetPlankState()
    {
        // Réinitialise l'état du plank à intact
        plank_full.SetActive(true);
        plank_break_middle.SetActive(false);
        plank_break_right.SetActive(false);
        plank_break_left.SetActive(false);
    }

    void ShowLoseText()
    {
        canMove = false; // Arrête le mouvement
        ShowAxoState(axoLose); // Affiche l'image de défaite

        // Réinitialise le plank à son état intact
        ResetPlankState();

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

    // Implémentation de l'interface IMinigameController
    public void GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
    {
        Debug.Log($"Génération du mini-jeu avec le seed {seed} et la difficulté {difficultyLevel}");
        Random.InitState(seed);

        // Ajustez les paramètres en fonction de la difficulté
        switch (difficultyLevel)
        {
            case MinigameDifficultyLevel.VeryEasy:
                moveSpeed = 50f;
                successNeeded = 1;
                break;
            case MinigameDifficultyLevel.Easy:
                moveSpeed = 75f;
                successNeeded = 2;
                break;
            case MinigameDifficultyLevel.Medium:
                moveSpeed = 100f;
                successNeeded = 3;
                break;
            case MinigameDifficultyLevel.Hard:
                moveSpeed = 125f;
                successNeeded = 4;
                break;
            case MinigameDifficultyLevel.VeryHard:
                moveSpeed = 150f;
                successNeeded = 5;
                break;
            case MinigameDifficultyLevel.Impossible:
                moveSpeed = 200f;
                successNeeded = 6;
                break;
            default:
                moveSpeed = 100f;
                successNeeded = 2;
                break;
        }

        // Réinitialisez l'état du jeu
        ResetPlankState();
        successCount = 0;
        failCount = 0;
        canMove = false;
    }

    public void InitializeMinigame()
    {
        Debug.Log("Initialisation du mini-jeu...");
        ShowAxoState(axoWaiting);
        StartCoroutine(StartGame());
    }

    public void StartMinigame()
    {
        Debug.Log("Démarrage du mini-jeu...");
        canMove = true;
    }
}
