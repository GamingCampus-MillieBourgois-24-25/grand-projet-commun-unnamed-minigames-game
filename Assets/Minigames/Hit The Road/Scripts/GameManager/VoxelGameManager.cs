using System.Collections;
using UnityEngine;

public class VoxelGameManager : MonoBehaviour
{
    public static VoxelGameManager Instance;
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public Transform defeatWaypoint;
    public Transform policeCarWaypoint; // Waypoint pour le PoliceCar
    public GameObject policeCarPrefab; // Prefab du PoliceCar

    private bool isGameOver = false; // Indique si le jeu est terminé
    private bool hasWon;

    void Awake()
    {
        Instance = this;
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    private IEnumerator SlowDownMovingTiles()
    {
        MovingTile[] tiles = FindObjectsOfType<MovingTile>();
        float duration = 2.5f; // Durée pour ralentir les tuiles
        float elapsedTime = 0f;

        // Stocker les vitesses initiales des tuiles
        float[] initialSpeeds = new float[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            initialSpeeds[i] = tiles[i].getSpeed();
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            foreach (var tile in tiles)
            {
                int index = System.Array.IndexOf(tiles, tile);
                float newSpeed = Mathf.Lerp(initialSpeeds[index], 0f, t);
                tile.setSpeed(newSpeed);
            }

            yield return null;
        }

        // Assurer que la vitesse est bien à 0
        foreach (var tile in tiles)
        {
            tile.setSpeed(0f);
        }
    }

    public void PlayerFails()
    {
        if (isGameOver) return; // Empêche l'exécution si le jeu est déjà terminé
        isGameOver = true;

        Debug.Log("Défaite !");
        defeatPanel.SetActive(true);
        victoryPanel.SetActive(false);

        StartCoroutine(SlowDownMovingTiles());

        PlayerBike playerBike = FindObjectOfType<PlayerBike>();
        if (playerBike != null && defeatWaypoint != null)
        {
            playerBike.MoveToWaypoint(defeatWaypoint.position, 2.4f);
        }

        // Faire apparaître le PoliceCar
        if (policeCarPrefab != null && policeCarWaypoint != null)
        {
            GameObject policeCar = Instantiate(policeCarPrefab, policeCarWaypoint.position, Quaternion.identity);
            StartCoroutine(MovePoliceCarTowardsPlayer(policeCar));
        }

        MiniGameManager.Instance.isWin = false;
        MiniGameManager.Instance.MiniGameFinished(false);
    }

    public void PlayerWins()
    {
        if (isGameOver) return; // Empêche l'exécution si le jeu est déjà terminé
        isGameOver = true;

        Debug.Log("Victoire !");
        defeatPanel.SetActive(false);
        victoryPanel.SetActive(true);

        RivalBike rival = FindObjectOfType<RivalBike>();
        if (rival != null)
        {
            rival.ExplodeAndEject();
        }

        MiniGameManager.Instance.isWin = true;
        MiniGameManager.Instance.MiniGameFinished(true);
    }

    private IEnumerator MovePoliceCarTowardsPlayer(GameObject policeCar)
    {
        float duration = 5f; // Temps pour atteindre le joueur
        float elapsedTime = 0f;
        Vector3 startPosition = policeCar.transform.position;
        Vector3 targetPosition = FindObjectOfType<PlayerBike>().transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            policeCar.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }
}
