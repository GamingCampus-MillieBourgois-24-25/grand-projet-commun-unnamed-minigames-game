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

    [Header("Images d'�tats")]
    public GameObject axoVictory; // PNG pour la victoire
    public GameObject axoLose; // PNG pour la d�faite
    public GameObject axoWaiting; // PNG pour l'attente du premier clic
    public GameObject axoPointerMove; // PNG pour le mouvement du pointeur
    public GameObject axoSuccess; // PNG pour une r�ussite

    [Header("Param�tres")]
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float speedIncrease = 10f;
    [SerializeField] private float shakeDuration = 0.2f; // Dur�e de la vibration
    [SerializeField] private float shakeMagnitude = 0.1f; // Intensit� de la vibration

    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private bool canMove = false;
    private int successCount = 0;
    private int successNeeded = 3;
    private int failCount = 0; // Compteur d'�checs

    private Transform mainCameraTransform;
    private Vector3 originalCameraPosition;
    private Camera mainCamera;
    private Volume postProcessingVolume;
    private DepthOfField depthOfField;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;

        // R�cup�re la cam�ra principale
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
        originalCameraPosition = mainCameraTransform.position;

        // Configure le post-traitement
        postProcessingVolume = mainCamera.GetComponent<Volume>();
        if (postProcessingVolume != null)
        {
            postProcessingVolume.profile.TryGet(out depthOfField);
        }

        // Assurez-vous que le texte "Lose" est d�sactiv� au d�but
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

            // D�clenche une vibration de la cam�ra
            StartCoroutine(ShakeCamera());

            Debug.Log($"Succ�s {successCount}/{successNeeded}");

            // Affiche l'image de r�ussite
            ShowAxoState(axoSuccess);

            // D�clenche le zoom imm�diatement apr�s 2 r�ussites
            if (successCount == 2)
            {
                StartCoroutine(ZoomAndFocusEffect());
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

        // D�clenche le zoom et les effets
        yield return StartCoroutine(ZoomAndFocusEffect());

        yield return new WaitForSeconds(2f);
        LoadNextMiniGame();
    }



    IEnumerator ZoomAndFocusEffect()
    {
        float targetOrthographicSize = 5f; // Taille orthographique cible pour le zoom
        float zoomDuration = 1f; // Dur�e du zoom
        float elapsedTime = 0f;

        float initialOrthographicSize = mainCamera.orthographicSize;

        // V�rifie si l'effet Depth of Field est disponible
        if (depthOfField != null)
        {
            depthOfField.active = true; // Active l'effet
        }

        // Param�tres initiaux et cibles pour le flou
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

        Debug.Log("Zoom et effet de mise au point termin�s.");
    }

    void LoadNextMiniGame()
    {
        Debug.Log("Chargement du prochain mini-jeu...");
        // Ici, ajoute le code pour charger ton prochain mini-jeu
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
