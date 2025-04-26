using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PointerController : MonoBehaviour, IMinigameController
{

    // Ctrl + M + O pour d�plier toutes les r�gions
    #region PROPERTIES----------------------------------------------------------------------

    public MinigameObject breakThePlank;

    [Header("R�f�rences")]
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public Text startText;
    public Text victoryText;
    public Text loseText; // R�f�rence au texte "Lose"

    [Header("R�f�rences suppl�mentaires")]
    public LevelManager levelManager; // R�f�rence au LevelManager
    public SpriteAnimator spriteAnimator; // R�f�rence au script SpriteAnimator

    [Header("Images d'�tats")]
    public GameObject axoVictory; // PNG pour la victoire
    public GameObject axoLose; // PNG pour la d�faite
    public GameObject axoWaiting; // PNG pour l'attente du premier clic
    public GameObject axoPointerMove; // PNG pour le mouvement du pointeur
    public GameObject axoSuccess; // PNG pour une r�ussite

    [Header("Plank UI Elements")]
    public RectTransform plankFull; // Plank intact (UI)
    public RectTransform plankBreakMiddle; // Plank cass� au milieu (UI)
    public RectTransform plankBreakRight; // Plank cass� � droite (UI)
    public RectTransform plankBreakLeft; // Plank cass� � gauche (UI)

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
    bool moveArrow = true;


    #endregion
    #region LIFECYCLE-----------------------------------------------------------------------

    void Start()
    {
        if (!ValidateReferences()) return;

        // Assurez-vous que le texte "Lose" est d�sactiv� au d�but
        if (loseText != null)
            loseText.gameObject.SetActive(false);

        // Affiche l'image d'attente
        ShowAxoState(axoWaiting);

        // Affiche le plank intact au d�but
        ResetPlankState();

        // Lance l'animation de rotation au d�but du jeu
        if (spriteAnimator != null)
        {
            spriteAnimator.StartRotation();
        }

        StartCoroutine(StartGame());
    }

    void Update()
    {
        if (!canMove) return;

        // Affiche l'image de mouvement du pointeur
        ShowAxoState(axoPointerMove);

        if (moveArrow)
            pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
            targetPosition = pointB.position;
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
            targetPosition = pointA.position;

        if (Input.GetKeyDown(KeyCode.Space) || IsTouching())
            PlayAction();
    }

    #endregion
    #region METHODS-------------------------------------------------------------------------

    public void GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
    {
        Debug.Log($"G�n�ration du mini-jeu avec le seed {seed} et la difficult� {difficultyLevel}");
        Random.InitState(seed);

        // Ajustez les param�tres en fonction de la difficult�
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

        // R�initialisez l'�tat du jeu
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
        Debug.Log("D�marrage du mini-jeu...");
        canMove = true;
    }



    bool ValidateReferences()
    {
        // Validation des r�f�rences
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Les points A et B ne sont pas assign�s !");
            return false;
        }

        pointerTransform = GetComponent<RectTransform>();
        if (pointerTransform == null)
        {
            Debug.LogError("RectTransform manquant sur l'objet PointerController !");
            return false;
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
            return false;
        }

        return true;
    }
    bool IsTouching()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
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



    void ShowLoseText()
    {
        canMove = false; // Arr�te le mouvement
        ShowAxoState(axoLose); // Affiche l'image de d�faite

        // R�initialise le plank � son �tat intact
        ResetPlankState();

        // Lance l'animation de d�placement hors du champ de vision en cas de d�faite
        if (spriteAnimator != null)
        {
            spriteAnimator.MoveOutOfView();
        }

        if (loseText != null)
        {
            loseText.gameObject.SetActive(true); // Affiche le texte "Lose"
        }
        Debug.Log("Vous avez perdu !");
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

    void UpdatePlankState()
    {
        // R�initialise l'�tat des planks
        ResetPlankState();

        // D�termine la position de la Safe Zone
        float safeZoneX = safeZone.anchoredPosition.x;

        // D�finir des plages pr�cises pour chaque �tat
        if (safeZoneX < -300) // Zone tr�s � gauche
        {
            plankBreakLeft.gameObject.SetActive(true);
            plankFull.gameObject.SetActive(false); // D�sactive plankFull
        }
        else if (safeZoneX >= -300 && safeZoneX < -100) // Zone l�g�rement � gauche
        {
            plankBreakLeft.gameObject.SetActive(true);
            plankFull.gameObject.SetActive(false); // D�sactive plankFull
        }
        else if (safeZoneX >= -100 && safeZoneX <= 100) // Zone centrale
        {
            plankBreakMiddle.gameObject.SetActive(true);
            plankFull.gameObject.SetActive(false); // D�sactive plankFull
        }
        else if (safeZoneX > 100 && safeZoneX <= 300) // Zone l�g�rement � droite
        {
            plankBreakRight.gameObject.SetActive(true);
            plankFull.gameObject.SetActive(false); // D�sactive plankFull
        }
        else if (safeZoneX > 300) // Zone tr�s � droite
        {
            plankBreakRight.gameObject.SetActive(true);
            plankFull.gameObject.SetActive(false); // D�sactive plankFull
        }
    }

    void ResetPlankState()
    {
        // D�sactive toutes les planches cass�es
        plankBreakMiddle.gameObject.SetActive(false);
        plankBreakRight.gameObject.SetActive(false);
        plankBreakLeft.gameObject.SetActive(false);

        // Active uniquement la planche intacte par d�faut
        plankFull.gameObject.SetActive(true);
    }


    #endregion
    #region API-----------------------------------------------------------------------------



    #endregion
    #region COROUTINES----------------------------------------------------------------------

    IEnumerator StartGame()
    {
        if (startText != null)
        {
            startText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            startText.gameObject.SetActive(false);
        }

        // Randomise la Safe Zone apr�s le texte "Start"
        if (levelManager != null)
        {
            levelManager.UpdateSafeZone();
        }

        // Attends un court instant avant de permettre le mouvement et les interactions
        yield return new WaitForSeconds(0.5f);

        canMove = true; // Active le mouvement du pointeur et les interactions
    }

    IEnumerator HammerEffect(System.Action callback)
    {
        moveArrow = false;
        Vector3 originalPosition = pointerTransform.position; // Sauvegarde la position initiale
        Vector3 recoilPosition = originalPosition + new Vector3(0, 50f, 0); // Position l�g�rement au-dessus
        Vector3 hammerPosition = originalPosition + new Vector3(0, -50f, 0); // Position l�g�rement en dessous

        float duration = 0.2f; // Dur�e pour chaque �tape (recul et descente)

        // �tape 1 : Monte l�g�rement pour l'effet de recul
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            pointerTransform.position = Vector3.Lerp(originalPosition, recoilPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pointerTransform.position = recoilPosition; // Assure la position finale

        // �tape 2 : Descend pour l'effet de marteau
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

        // �tape 3 : Remonte � la position initiale
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

    IEnumerator ShowVictoryAndChangeGame()
    {
        canMove = false;

        // Affiche l'image de r�ussite (axoSuccess)
        ShowAxoState(axoSuccess);

        // Lance l'animation de sautillement en cas de victoire
        if (spriteAnimator != null)
        {
            spriteAnimator.StartBounce();
        }

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

    #endregion

















}