using UnityEngine;
using System.Collections;
using Axoloop.Global;
using AxoLoop.Minigames.HitTheRoad;

public class PlayerBike : SingletonMB<PlayerBike>
{
    public float moveSpeed = 10f; // Vitesse de déplacement vers l'avant
    public float turnSpeed = 4f; // Vitesse de déplacement latéral
    public float leanAngle = 30f; // Angle d'inclinaison maximal
    public float leanSmoothness = 5f; // Vitesse de transition de l'inclinaison

    public GameObject movingObject; // L'objet qui doit suivre la trajectoire
    public Transform targetPoint; // Le point intermédiaire dans la scène
    public float moveSpeedToTarget = 5f; // Vitesse de déplacement vers le TargetPoint
    public float moveSpeedToPlayer = 7f; // Vitesse de déplacement vers le PlayerBike

    private bool isTurningLeft = false;
    private bool isTurningRight = false;
    private bool hasCollided = false;
    private bool hasTurned = false; // Nouvelle variable pour limiter à un seul virage

    private float currentLeanAngle = 0f; // Inclinaison actuelle de la moto
    private Rigidbody rb; // Référence au Rigidbody

    bool accident = false;
    float targetLeanAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody manquant sur PlayerBike !");
        }
    }

    void FixedUpdate()
    {
        if (!hasCollided) // Empêche les mouvements après une collision
        {
            HandleTurning();
        }
    }

    private void HandleTurning()
    {
        targetLeanAngle = 0f;
        Vector3 lateralMovement = Vector3.zero;

        if (isTurningLeft)
        {
            lateralMovement = Vector3.left * turnSpeed * Time.fixedDeltaTime;
            targetLeanAngle = leanAngle;
        }
        else if (isTurningRight)
        {
            lateralMovement = Vector3.right * turnSpeed * Time.fixedDeltaTime;
            targetLeanAngle = -leanAngle;
        }

        rb.MovePosition(rb.position + lateralMovement);
        currentLeanAngle = Mathf.Lerp(currentLeanAngle, targetLeanAngle, leanSmoothness * Time.fixedDeltaTime);
        Quaternion targetRotation = Quaternion.Euler(0, 0, currentLeanAngle);
        rb.MoveRotation(targetRotation);
    }

    public void StartTurnLeft()
    {
        if (!hasCollided && !hasTurned) // Vérifie si le joueur n'a pas encore tourné
        {
            isTurningLeft = true;
            isTurningRight = false;
            hasTurned = true; // Marque que le joueur a tourné
        }
    }

    public void StartTurnRight()
    {
        if (!hasCollided && !hasTurned) // Vérifie si le joueur n'a pas encore tourné
        {
            isTurningRight = true;
            isTurningLeft = false;
            hasTurned = true; // Marque que le joueur a tourné
        }
    }

    public void StopTurning()
    {
        if(!accident)
            rb.MoveRotation(new Quaternion(0, 0, 0, 1));
        isTurningLeft = false;
        isTurningRight = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollided) return;

        var time = System.DateTime.Now;
        //

        StartCoroutine(ScreenShake(0.5f, 0.2f)); // Durée : 0.5s, Intensité : 0.2
        StopTurning();

        // joueur est rentré dans la moto adverse, victoire
        if (other.CompareTag("RivalBike"))
        {
            hasCollided = true;
            VoxelGameManager.Instance.PlayerWins();
        }

        // joueur a fait une sortie de route, défaite !
        else if (other.CompareTag("Boundary"))
        {
            accident = true;
            hasCollided = true;
            VoxelGameManager.Instance.PlayerFails();

            // Appeler FinalAcceleration sur RivalBike
            if (RivalBike.Instance != null)
            {
                RivalBike.Instance.StartCoroutine(RivalBike.Instance.FinalAcceleration());
            }

            
            // Déplacer l'objet vers le TargetPoint puis devant le PlayerBike
            if (movingObject != null && targetPoint != null)
            {
                StartCoroutine(MoveObjectToTargetAndPlayer());
            }
            
            var timeAfter = System.DateTime.Now;
            var timeSpan = timeAfter - time;
            Debug.Log("LagDuration : " + timeSpan.TotalMilliseconds + "ms");
        }
    }

    public void StopPlayer()
    {
        HitTheRoadController.Instance.DisableButtons();
        // Ralentir et arrêter les routes
        MovingTile[] movingTiles = FindObjectsOfType<MovingTile>();
        foreach (MovingTile tile in movingTiles)
        {
            tile.SlowDownAndStop(accident ? 0.5f : 2f); // Ralentir sur 2 secondes
        }
    }

    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 originalPosition = cameraTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            cameraTransform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            yield return null;
        }

        // Réinitialiser la position de la caméra
        cameraTransform.localPosition = originalPosition;
    }

    private IEnumerator MoveObjectToTargetAndPlayer()
    {
        // Étape 1 : Déplacer l'objet vers le TargetPoint
        while (Vector3.Distance(movingObject.transform.position, targetPoint.position) > 0.1f)
        {
            movingObject.transform.position = Vector3.Lerp(
                movingObject.transform.position,
                targetPoint.position,
                moveSpeedToTarget * Time.deltaTime
            );
            yield return null;
        }
    }
}
