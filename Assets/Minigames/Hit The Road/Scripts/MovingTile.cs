using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingTile : MonoBehaviour
{
    public float speed = 15f; // Vitesse initiale de la route
    private Coroutine slowDownCoroutine;
    private Rigidbody rb;
    bool displaced = false; // Indique si le tile a été déplacé 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + new Vector3(0, 0, -speed * Time.fixedDeltaTime));
    }

    private void Update()
    {
        displaced = false;
    }

    //displace the tile to the start of the road 100 units forward on the z axis  
    void TeleportToPointZero()
    {
        if ((displaced)) return;

        rb.interpolation = RigidbodyInterpolation.None;
        Vector3 newPosition = transform.position; 
        newPosition.z += 100f;
        transform.position = newPosition; 
        rb.position = newPosition; // Met à jour la position du Rigidbody
        displaced = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPoint"))
        {
            TeleportToPointZero();
            Debug.Log("Collision entre " + gameObject.name + " et " + other.gameObject.name);
        }
    }

    public float getSpeed()
    {
        return speed;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SlowDownAndStop(float duration)
    {
        if (slowDownCoroutine != null)
        {
            StopCoroutine(slowDownCoroutine);
        }
        slowDownCoroutine = StartCoroutine(SlowDownCoroutine(duration));
    }

    private IEnumerator SlowDownCoroutine(float duration)
    {
        float initialSpeed = speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            speed = Mathf.Lerp(initialSpeed, 0, elapsedTime / duration);
            yield return null;
        }

        speed = 0; // Assurez-vous que la vitesse est bien à 0 à la fin
    }
}
