using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    [Header("Références")]
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public Text startText;
    public Text victoryText;
    public Text loseText; // Référence au texte "Lose"

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

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;

        // Récupère la caméra principale
        mainCameraTransform = Camera.main.transform;
        originalCameraPosition = mainCameraTransform.position;

        // Assurez-vous que le texte "Lose" est désactivé au début
        if (loseText != null)
            loseText.gameObject.SetActive(false);

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

            if (successCount >= successNeeded)
                StartCoroutine(ShowVictoryAndChangeGame());
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
        victoryText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        LoadNextMiniGame();
    }

    void LoadNextMiniGame()
    {
        Debug.Log("Chargement du prochain mini-jeu...");
        // Ici, ajoute le code pour charger ton prochain mini-jeu
        // Exemple si tu veux charger une nouvelle scène :
        // SceneManager.LoadScene("MAIN Hit the road");
    }
}
