using System.Collections;
using UnityEngine;

public class RivalBike : MonoBehaviour
{
    public float speed = 12f;
    private float laneOffset = 2.5f;
    private bool willTurn = false;
    private float finalLane;
    private bool isTurning = false;
    private bool hasDecided = false;

    void Start()
    {
        int spawnType = Random.Range(0, 3);
        if (spawnType == 0) // gauche
        {
            transform.position = new Vector3(-laneOffset, transform.position.y, transform.position.z);
            finalLane = -laneOffset;
        }
        else if (spawnType == 1) // droite
        {
            transform.position = new Vector3(laneOffset, transform.position.y, transform.position.z);
            finalLane = laneOffset;
        }
        else // centre = décider plus tard
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            willTurn = true;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (willTurn && hasDecided && !isTurning)
        {
            isTurning = true;
            StartCoroutine(SmoothTurn(finalLane));
        }
    }

    public void DecideTurnDirection()
    {
        if (!hasDecided)
        {
            finalLane = Random.value > 0.5f ? laneOffset : -laneOffset;
            hasDecided = true;
        }
    }

    public bool ShouldDecideTurn()
    {
        return willTurn && !hasDecided;
    }

    private IEnumerator SmoothTurn(float targetX)
    {
        float duration = 0.6f;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetX, startPosition.y, startPosition.z + 3f);

        float maxLeanAngle = 15f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curvedT = Mathf.SmoothStep(0, 1, t);
            transform.position = Vector3.Lerp(startPosition, endPosition, curvedT);

            float leanDirection = Mathf.Sign(targetX - startPosition.x);
            float currentLean = Mathf.Lerp(0, leanDirection * maxLeanAngle, Mathf.Sin(Mathf.PI * curvedT));
            transform.rotation = Quaternion.Euler(0, 0, -currentLean);

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public float GetFinalLane()
    {
        return finalLane;
    }

    // Détecter si le RivalBike a dépassé un point de défaite
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DefeatTrigger"))  // Vérifie si c'est le trigger de défaite
        {
            VoxelGameManager.Instance.PlayerFails();  // Appelle la méthode de défaite
        }
    }
}
