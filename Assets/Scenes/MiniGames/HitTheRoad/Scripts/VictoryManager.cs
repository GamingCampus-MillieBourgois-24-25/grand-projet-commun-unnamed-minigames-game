using UnityEngine;

public class VictoryCameraZoom : MonoBehaviour
{
    public Transform playerBike;          // À assigner dans l'inspector
    public Vector3 offset = new Vector3(0, 2, -3); // Position finale relative au PlayerBike
    public float zoomDuration = 1.5f;

    private bool isZooming = false;
    private float timer = 0f;
    private Vector3 startPos;

    void StartZoom()
    {
        startPos = transform.position;
        isZooming = true;
        timer = 0f;
    }

    void Update()
    {
        if (!isZooming) return;

        timer += Time.deltaTime;
        float t = timer / zoomDuration;
        if (t >= 1f)
        {
            t = 1f;
            isZooming = false;
        }

        Vector3 targetPos = playerBike.position + offset;
        transform.position = Vector3.Lerp(startPos, targetPos, t);
        transform.LookAt(playerBike);
    }

    public void TriggerVictoryZoom()
    {
        StartZoom();
    }
}
 