using UnityEngine;

public class PlayerBike : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float laneOffset = 2.5f; // Distance entre les voies
    private bool hasMoved = false;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                CheckResult();
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

    void CheckResult()
    {
        if (VoxelGameManager.Instance.IsCorrectChoice(this.transform.position.x))
        {
            VoxelGameManager.Instance.PlayerWins();
        }
        else
        {
            VoxelGameManager.Instance.PlayerFails();
        }
    }
}
