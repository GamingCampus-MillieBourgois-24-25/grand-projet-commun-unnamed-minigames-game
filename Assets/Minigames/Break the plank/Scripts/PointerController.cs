using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PointerController : MonoBehaviour
{
    [Header("R�f�rences")]
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public Text startText;
    public Text victoryText;
    public Text loseText; // R�f�rence au texte "Lose"
    [Header("R�f�rences suppl�mentaires")]
    public LevelManager levelManager; // R�f�rence au LevelManager

    [Header("Images d'�tats")]
    public GameObject axoVictory; // PNG pour la victoire
    public GameObject axoLose; // PNG pour la d�faite
    public GameObject axoWaiting; // PNG pour l'attente du premier clic
    public GameObject axoPointerMove; // PNG pour le mouvement du pointeur
    public GameObject axoSuccess; // PNG pour une r�ussite

    [Header("Plank Sprites")]
    public GameObject plank_full; // Plank intact
    public GameObject plank_break_middle; // Plank cass� au milieu
    public GameObject plank_break_right; // Plank cass� � droite
    public GameObject plank_break_left; // Plank cass� � gauche

    [Header("Param�tres")]
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float speedIncrease = 10f;
    [SerializeField] private float shakeDuration = 0.15f; // Dur�e de la vibration
    [SerializeField] private float shakeMagnitude = 0.08f; // Intensit� de la vibration

    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private bool canMove = false;
    private int successCount = 0;
    private int successNeeded = 2;
    private int failCount = 0; // Compteur d'�checs

    private Transform mainCameraTransform;
    private Vector3 originalCameraPosition;
    private Camera mainCamera;
    private Volume postProcessingVolume;
    private DepthOfField depthOfField;

    void Start()
    {
        // Validation des r�f�rences
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Les points A et B ne sont pas assign�s !");
            return;
        }

        pointerTransform = GetComponent<RectTransform>();
        if (pointerTransform == null)
        {
            Debug.LogError("RectTransform manquant sur l'objet PointerController !");
            return;
        }

        targetPosition = pointB.position;

        // R�cup�re la cam�ra principale
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
            Debug.LogError("Cam�ra principale introuvable !");
        }

        // Assurez-vous que le texte "Lose" est d�sactiv� au d�but
        if (loseText != null)
            loseText.gameObject.SetActive(false);

        // Affiche l'image d'attente
        ShowAxoState(axoWaiting);

        // Affiche le plank intact au d�but
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
        Vector3 hammerPosition = originalPosition + new Vector3(0, -40f, 0); // Position l�g�rement en dessous

        // Descend le pointeur
        pointerTransform.position = hammerPosition;
        yield return new WaitForSeconds(0.1f); // Attente courte pour l'effet de descente

        // Remonte le pointeur � sa position initiale
        pointerTransform.position = originalPosition;
    }

    void CheckSuccess()
    {
        // Lance l'effet de marteau
        StartCoroutine(HammerEffect());

        if (safeZone == null)
        {
            Debug.LogError("Safe Zone non assign�e !");
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            successCount++;
            moveSpeed += speedIncrease;

            // D�clenche une vibration de la cam�ra
            StartCoroutine(ShakeCamera());

            Debug.Log($"Succ�s {successCount}/{successNeeded}");

            // Affiche l'image de r�ussite
            ShowAxoState(axoSuccess);

            // Affiche le plank correspondant uniquement apr�s la 2�me frappe r�ussie
            if (successCount == successNeeded)
            {
                UpdatePlankState();
                StartCoroutine(FadeOutSafeZone()); // Lance la disparition de la Safe Zone
            }

            // Change la position de la safeZone via le LevelManager uniquement si le nombre de succ�s est inf�rieur � successNeeded
            if (levelManager != null && successCount < successNeeded)
            {
                levelManager.UpdateSafeZone(); // Met � jour la Safe Zone
            }
            else if (successCount >= successNeeded)
            {
                Debug.Log("La Safe Zone ne se d�place plus.");
            }

            // V�rifie si le joueur a gagn�
            if (successCount >= successNeeded)
            {
                StartCoroutine(ShowVictoryAndChangeGame());
            }
        }
        else
        {
            failCount++; // Incr�mente le compteur d'�checs
            Debug.Log($"�chec ! Nombre d'�checs : {failCount}");

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
            // Ajoute un CanvasGroup si non pr�sent
            canvasGroup = safeZone.gameObject.AddComponent<CanvasGroup>();
        }

        float duration = 0.7f; // Dur�e de la disparition
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration); // R�duit l'opacit�
            yield return null;
        }

        canvasGroup.alpha = 0f; // Assure que l'opacit� est � 0
        safeZone.gameObject.SetActive(false); // D�sactive la Safe Zone apr�s la disparition
    }


    void UpdatePlankState()
    {
        // D�sactive tous les GameObjects
        plank_full.SetActive(false);
        plank_break_middle.SetActive(false);
        plank_break_right.SetActive(false);
        plank_break_left.SetActive(false);

        // D�termine la position de la Safe Zone
        float safeZoneX = safeZone.anchoredPosition.x;

        // D�finir des plages pr�cises pour chaque sprite
        if (safeZoneX < -300) // Zone tr�s � gauche
        {
            plank_break_left.SetActive(true);
        }
        else if (safeZoneX >= -300 && safeZoneX < -100) // Zone l�g�rement � gauche
        {
            plank_break_left.SetActive(true);
        }
        else if (safeZoneX >= -100 && safeZoneX <= 100) // Zone centrale
        {
            plank_break_middle.SetActive(true);
        }
        else if (safeZoneX > 100 && safeZoneX <= 300) // Zone l�g�rement � droite
        {
            plank_break_right.SetActive(true);
        }
        else if (safeZoneX > 300) // Zone tr�s � droite
        {
            plank_break_right.SetActive(true);
        }
    }


    void ResetPlankState()
    {
        // R�initialise l'�tat du plank � intact
        plank_full.SetActive(true);
        plank_break_middle.SetActive(false);
        plank_break_right.SetActive(false);
        plank_break_left.SetActive(false);
    }

    void ShowLoseText()
    {
        canMove = false; // Arr�te le mouvement
        ShowAxoState(axoLose); // Affiche l'image de d�faite

        // R�initialise le plank � son �tat intact
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

        // R�initialise la position de la cam�ra
        mainCameraTransform.position = originalCameraPosition;
    }

    IEnumerator ShowVictoryAndChangeGame()
    {
        canMove = false;

        // Affiche l'image de r�ussite (axoSuccess)
        ShowAxoState(axoSuccess);
        yield return new WaitForSeconds(2f); // Attente de 2 secondes

        // Affiche l'image de victoire (axoVictory)
        ShowAxoState(axoVictory);

        // Affiche le texte de victoire en m�me temps
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
        // Exemple si tu veux charger une nouvelle sc�ne :
        // SceneManager.LoadScene("MAIN Hit the road");
    }

    void ShowAxoState(GameObject activeAxo)
    {
        // D�sactive toutes les images
        if (axoVictory != null) axoVictory.SetActive(false);
        if (axoLose != null) axoLose.SetActive(false);
        if (axoWaiting != null) axoWaiting.SetActive(false);
        if (axoPointerMove != null) axoPointerMove.SetActive(false);
        if (axoSuccess != null) axoSuccess.SetActive(false);

        // Active uniquement l'image correspondant � l'�tat actuel
        if (activeAxo != null) activeAxo.SetActive(true);
    }
}
