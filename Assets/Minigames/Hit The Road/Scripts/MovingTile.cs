using System.Collections;
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    public float speed = 15f; // Vitesse initiale de la route
    private Coroutine slowDownCoroutine;

    void Update()
    {
        transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
    }

    void TeleportToPointZero()
    {
        transform.position += new Vector3(0, 0, 150);
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
