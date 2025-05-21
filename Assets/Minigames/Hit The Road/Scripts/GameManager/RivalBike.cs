using Axoloop.Global;
using AxoLoop.Minigames.HitTheRoad;
using System.Collections;
using UnityEngine;

public class RivalBike : SingletonMB<RivalBike>
{
    public Rigidbody rb; // Référence au Rigidbody du RivalBike
    public GameObject physicBike, wholeBike;

    public GameTrigger turnTrigger, defeatTrigger;

    public float speed = 12f;
    private float laneOffset = 4f;
    private bool willTurn = false;
    public float finalLane;
    private bool hasDecided = false;

    bool accident = false;
    
    public GameObject smokeParticles; // Référence au système de particules
    public ParticleSystem explosionParticles; // Référence au système de particules d'explosion

    void Start()
    {        
        turnTrigger.EnterSubscribe(TryDecideTurn);
        defeatTrigger.EnterSubscribe(EndTriggerReached);

        Spawn();

        rb = GetComponent<Rigidbody>();
        physicBike.SetActive(false); // Désactiver le modèle physique au départ
    }

    void FixedUpdate()
    {
        if (!accident)
        {
            rb.MovePosition(rb.position + Vector3.forward * speed * Time.fixedDeltaTime); 
        }
    }


    void Spawn()
    {
        // Si speed < 13 alors difficulté facile et on empeche la moto de rester au centre. C'est dégeulasse, il faudra évidemment changer ça.
        int spawnType = Random.Range(0, speed < 13 ? 2 : 3);
        if (spawnType == 0) // gauche
        {
            rb.position = new Vector3(-laneOffset, transform.position.y, transform.position.z);
            finalLane = -laneOffset;
        }
        else if (spawnType == 1) // droite
        {
            rb.position = new Vector3(laneOffset, transform.position.y, transform.position.z);
            finalLane = laneOffset;
        }
        else // centre = décider plus tard
        {
            rb.position = new Vector3(0, transform.position.y, transform.position.z);
            willTurn = true;
        }
    }

    void EndTriggerReached()
    {
        VoxelGameManager.Instance.PlayerFails();  // Appelle la méthode de défaite
        StartCoroutine(FinalAcceleration()); // Lance l'accélération finale
    }

    void TryDecideTurn()
    {
        if (ShouldDecideTurn())
        {
            turnTrigger.EnterUnsubscribe(TryDecideTurn);
            DecideTurnDirection();
            willTurn = false; // Réinitialiser la variable willTurn après la décision
            StartCoroutine(SmoothTurn(finalLane));
        }
    }
    void DecideTurnDirection()
    {
        if (!hasDecided)
        {
            finalLane = Random.value > 0.5f ? laneOffset : -laneOffset;
            hasDecided = true;
        }
    }

    bool ShouldDecideTurn()
    {
        return willTurn && !hasDecided;
    }

    private IEnumerator SmoothTurn(float targetX)
    {
        float duration = 0.4f; // Réduire la durée pour un virage plus rapide
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetX, startPosition.y, startPosition.z + 8f); // Avance légèrement pendant le virage

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
        rb.rotation = transform.rotation;
        rb.position = transform.position;
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
        if (HitTheRoadController.Instance.loose) yield break;

        HitTheRoadController.Instance.loose = true; // Indique que le joueur a perdu

        PlayerBike.Instance.StopPlayer(); // Arrête le joueur
        GetComponent<Collider>().enabled = false; // Désactive le collider pour éviter les collisions pendant l'accélération
        float duration = 0.3f; // Durée de l'accélération
        float elapsedTime = 0f;
        float initialSpeed = speed;
        float finalSpeed = speed * 3.3f; // Double la vitesse pour l'accélération finale

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            speed = Mathf.Lerp(initialSpeed, -initialSpeed, t);
            yield return null;
        }
        elapsedTime = 0;
        // Déclenche les particules de fumée au début de l'accélération
        TriggerSmokeParticles();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curvedT = EaseInOutBack(t); // Utilise la fonction d'animation EaseInOutBack
            speed = Mathf.Lerp(0, finalSpeed, curvedT);
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
        accident = true;
        StartCoroutine(ExplodeAndEjectCoroutine());
    }

    private IEnumerator ExplodeAndEjectCoroutine()
    {
        // Déclenche les particules d'explosion
        if (explosionParticles != null)
        {
            explosionParticles.Play();
        }

        // Applique une force d'éjection au RivalBike
        if (rb != null)
        {
            rb.isKinematic = false; // Assurez-vous que le Rigidbody n'est pas kinematic
            rb.AddForce(new Vector3(0, 20, 0), ForceMode.Impulse); // Applique une force vers le haut et vers l'arrière
            rb.AddTorque(new Vector3(25, 25, 25)); // Ajoute une rotation pour un effet plus dramatique
        }
        yield return null;

        wholeBike.gameObject.SetActive(false);
        physicBike.gameObject.SetActive(true);
        physicBike.transform.SetParent(null); 

        //// Désactive le RivalBike après un court délai
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false);
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}
