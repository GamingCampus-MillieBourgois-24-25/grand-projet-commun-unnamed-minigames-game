using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    [Header("Références")]
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public Text startText;
    public Text victoryText;

    [Header("Paramètres")]
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float speedIncrease = 10f;
    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private bool canMove = false;
    private int successCount = 0;
    private int successNeeded = 3;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
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

            // Déclenche une vibration
            Handheld.Vibrate();

            Debug.Log($"Succès {successCount}/{successNeeded}");

            if (successCount >= successNeeded)
                StartCoroutine(ShowVictoryAndChangeGame());
        }
        else
        {
            Debug.Log("Échec !");
        }
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
        // SceneManager.LoadScene("NomDeLaSceneSuivante");
    }
}
