using System.Collections;
using UnityEngine;

public class PlayerBike : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float laneOffset = 2.5f;
    [SerializeField] private Vector3 targetPosition;
    private bool isMoving = false;
    private bool hasMoved = false;
    private bool hasCollided = false;

    private float defeatCheckDelay = 1f;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f && RivalBike.Instance.rivalPassed)
            {
                isMoving = false;
                Invoke(nameof(VoxelGameManager.Instance.PlayerFails), defeatCheckDelay);
            }
        }
    }

    public void MoveLeft()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            MoveTo(-laneOffset);
        }
    }

    public void MoveRight()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            MoveTo(laneOffset);
        }
    }

    private void MoveTo(float offsetX)
    {
        targetPosition = new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z + 3f);
        isMoving = true;
    }

    public void MoveToWaypoint(Vector3 waypoint, float duration)
    {
        StartCoroutine(MoveToWaypointCoroutine(waypoint, duration));
    }

    private IEnumerator MoveToWaypointCoroutine(Vector3 waypoint, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(startPosition, waypoint, t);
            yield return null;
        }

        transform.position = waypoint; // Assurer la position finale
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollided) return;

        if (other.CompareTag("RivalBike"))
        {
            hasCollided = true;
            CancelInvoke(nameof(VoxelGameManager.Instance.PlayerFails));
            VoxelGameManager.Instance.PlayerWins();
        }
    }

}
