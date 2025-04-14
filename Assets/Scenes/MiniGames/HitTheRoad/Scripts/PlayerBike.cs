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
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                Invoke(nameof(CheckForFail), defeatCheckDelay); // attends 1s avant de checker si t�as rat�
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

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollided) return;

        if (other.CompareTag("RivalBike"))
        {
            hasCollided = true;
            CancelInvoke(nameof(CheckForFail));
            VoxelGameManager.Instance.PlayerWins();
        }
    }

    private void CheckForFail()
    {
        if (hasCollided) return;

        RivalBike rival = FindObjectOfType<RivalBike>();
        if (rival == null) return;

        float rivalZ = rival.transform.position.z;
        float playerZ = transform.position.z;

        bool rivalHasPassed = rivalZ > playerZ + 1f;
        bool playerIsOnWrongLane = Mathf.Abs(transform.position.x - rival.GetFinalLane()) > 0.1f;

        if (rivalHasPassed || playerIsOnWrongLane)
        {
            VoxelGameManager.Instance.PlayerFails();
        }
    }
}
