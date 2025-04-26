using System.Collections;
using UnityEngine;

public class RivalBike : MonoBehaviour
{
    public static RivalBike Instance;
    
    public float speed = 12f;
    private float laneOffset = 2.5f;
    private bool willTurn = false;
    public float finalLane;
    private bool isTurning = false;
    private bool hasDecided = false;
    public bool rivalPassed;
    
    public GameObject smokeParticles; // Référence au système de particules
    public ParticleSystem explosionParticles; // Référence au système de particules d'explosion
    public Rigidbody rb; // Référence au Rigidbody du RivalBike

    void Start()
    {
        Instance = this;
        
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

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Démarrer immédiatement le virage si nécessaire
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
        float duration = 0.4f; // Réduire la durée pour un virage plus rapide
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetX, startPosition.y, startPosition.z + 5f); // Avance légèrement pendant le virage

        float maxLeanAngle = 30f; // Augmenter l'angle pour une inclinaison plus réaliste

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Utiliser une interpolation lissée pour un mouvement fluide
            float curvedT = Mathf.SmoothStep(0, 1, t);
            transform.position = Vector3.Lerp(startPosition, endPosition, curvedT);

            // Inclinaison de la moto pendant le virage
            float leanDirection = Mathf.Sign(targetX - startPosition.x);
            float currentLean = Mathf.Lerp(0, leanDirection * maxLeanAngle, curvedT);
            transform.rotation = Quaternion.Euler(0, 0, -currentLean);

            yield return null;
        }

        // Réinitialiser l'inclinaison après le virage
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }



    // Détecter si le RivalBike a dépassé un point de défaite
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DefeatTrigger"))  // Vérifie si c'est le trigger de défaite
        {
            rivalPassed = true;
            VoxelGameManager.Instance.PlayerFails();  // Appelle la méthode de défaite
            StartCoroutine(FinalAcceleration()); // Lance l'accélération finale
        }
        rivalPassed = false;
    }

    private void TriggerSmokeParticles()
    {
        if (smokeParticles != null)
        {
            Debug.Log("Déclenchement des particules de fumée");
            smokeParticles.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Le système de particules de fumée n'est pas assigné");
        }
    }

    public IEnumerator FinalAcceleration()
    {
        float duration = 0.5f; // Durée de l'accélération
        float elapsedTime = 0f;
        float initialSpeed = speed;
        float finalSpeed = speed * 1.3f; // Double la vitesse pour l'accélération finale

        // Déclenche les particules de fumée au début de l'accélération
        TriggerSmokeParticles();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curvedT = EaseInOutBack(t); // Utilise la fonction d'animation EaseInOutBack
            speed = Mathf.Lerp(initialSpeed, finalSpeed, curvedT);
            yield return null;
        }

        speed = finalSpeed; // Assure que la vitesse finale est bien appliquée
        
    }

    private float EaseInOutBack(float t)
    {
        float c1 = 1.70158f;
        float c2 = c1 * 1.525f;

        return t < 0.5
            ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
            : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
    }

    public void ExplodeAndEject()
    {
        StartCoroutine(ExplodeAndEjectCoroutine());
    }

    private IEnumerator ExplodeAndEjectCoroutine()
    {
        // Déclenche les particules d'explosion
        if (explosionParticles != null)
        {
            explosionParticles.Play();
        }

        // Attends un court instant pour l'effet d'explosion
        yield return new WaitForSeconds(0.1f);

        // Applique une force d'éjection au RivalBike
        if (rb != null)
        {
            rb.isKinematic = false; // Assurez-vous que le Rigidbody n'est pas kinematic
            rb.AddForce(new Vector3(0, 1000, -1000), ForceMode.Impulse); // Applique une force vers le haut et vers l'arrière
            rb.AddTorque(new Vector3(500, 500, 500)); // Ajoute une rotation pour un effet plus dramatique
        }

        // Désactive le RivalBike après un court délai
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}
