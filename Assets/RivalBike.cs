using UnityEngine;

public class RivalBike : MonoBehaviour
{
    public float speed = 12f;
    private float laneOffset = 2.5f;
    private bool willTurn = false;
    private float finalLane;
    private bool isTurning = false;

    void Start()
    {
        int spawnType = Random.Range(0, 3);
        if (spawnType == 0) // Spawn à gauche
        {
            transform.position = new Vector3(-laneOffset, transform.position.y, transform.position.z);
            finalLane = -laneOffset;
        }
        else if (spawnType == 1) // Spawn à droite
        {
            transform.position = new Vector3(laneOffset, transform.position.y, transform.position.z);
            finalLane = laneOffset;
        }
        else // Spawn au centre, puis tourne à la dernière seconde
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            willTurn = true;
            finalLane = Random.value > 0.5f ? laneOffset : -laneOffset;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (willTurn && transform.position.z > -2f && !isTurning)
        {
            isTurning = true;
            StartCoroutine(SmoothTurn(finalLane));
        }
    }

    private System.Collections.IEnumerator SmoothTurn(float targetX)
    {
        float duration = 0.4f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = new Vector3(Mathf.Lerp(startPosition.x, targetX, t), startPosition.y, startPosition.z);
            yield return null;
        }
    }

    public float GetFinalLane()
    {
        return finalLane;
    }
}
