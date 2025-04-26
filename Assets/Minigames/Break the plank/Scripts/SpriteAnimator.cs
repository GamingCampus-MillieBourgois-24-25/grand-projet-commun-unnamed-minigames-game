using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [Header("GameObjects � animer")]
    public List<GameObject> objectsToAnimate; // Liste des GameObjects � animer

    [Header("Param�tres d'animation")]
    public float rotationAngle = 15f; // Angle de rotation pour l'oscillation
    public float rotationSpeed = 2f; // Vitesse de l'oscillation
    public float bounceHeight = 30f; // Hauteur du saut pour l'animation de sautillement
    public float bounceDuration = 0.5f; // Dur�e d'un cycle de saut
    public float moveOutSpeed = 500f; // Vitesse de d�placement hors du champ de vision

    private bool isRotating = false; // Indique si l'animation de rotation est active
    private bool isBouncing = false; // Indique si l'animation de sautillement est active

    /// <summary>
    /// Lance l'animation d'oscillation (rotation gauche-droite).
    /// </summary>
    public void StartRotation()
    {
        if (!isRotating && !isBouncing) // Ne d�marre pas si une autre animation est active
        {
            isRotating = true;
            StartCoroutine(OscillateObjects());
        }
    }

    /// <summary>
    /// Lance l'animation de sautillement sur place.
    /// Arr�te l'oscillation si elle est active.
    /// </summary>
    public void StartBounce()
    {
        if (isRotating)
        {
            StopRotation(); // Arr�te l'oscillation avant de commencer le sautillement
        }

        if (!isBouncing)
        {
            isBouncing = true;
            StartCoroutine(BounceObjects());
        }
    }

    /// <summary>
    /// Lance l'animation de d�placement hors du champ de vision.
    /// </summary>
    public void MoveOutOfView()
    {
        StopRotation(); // Arr�te toute rotation
        isBouncing = false; // Arr�te le sautillement
        StartCoroutine(MoveObjectsOutOfView());
    }

    private IEnumerator OscillateObjects()
    {
        float elapsedTime = 0f;

        while (isRotating)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            float angle = Mathf.Sin(elapsedTime) * rotationAngle; // Oscillation entre -rotationAngle et +rotationAngle

            foreach (GameObject obj in objectsToAnimate)
            {
                if (obj != null)
                {
                    obj.transform.localRotation = Quaternion.Euler(0, 0, angle);
                }
            }

            yield return null;
        }

        // R�initialise la rotation � 0 lorsque l'oscillation s'arr�te
        foreach (GameObject obj in objectsToAnimate)
        {
            if (obj != null)
            {
                obj.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private IEnumerator BounceObjects()
    {
        float elapsedTime = 0f;
        Vector3[] originalPositions = new Vector3[objectsToAnimate.Count];

        // Sauvegarde des positions initiales
        for (int i = 0; i < objectsToAnimate.Count; i++)
        {
            if (objectsToAnimate[i] != null)
            {
                originalPositions[i] = objectsToAnimate[i].transform.localPosition;
            }
        }

        while (isBouncing)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.PingPong(elapsedTime / bounceDuration, 1f);

            for (int i = 0; i < objectsToAnimate.Count; i++)
            {
                if (objectsToAnimate[i] != null)
                {
                    Vector3 originalPosition = originalPositions[i];
                    objectsToAnimate[i].transform.localPosition = originalPosition + new Vector3(0, Mathf.Lerp(0, bounceHeight, t), 0);
                }
            }

            yield return null;
        }

        // R�initialise les positions � leur �tat d'origine
        for (int i = 0; i < objectsToAnimate.Count; i++)
        {
            if (objectsToAnimate[i] != null)
            {
                objectsToAnimate[i].transform.localPosition = originalPositions[i];
            }
        }
    }

    private IEnumerator MoveObjectsOutOfView()
    {
        Vector3 direction = new Vector3(0, -1, 0); // D�placement vers le bas
        while (true)
        {
            foreach (GameObject obj in objectsToAnimate)
            {
                if (obj != null)
                {
                    obj.transform.localPosition += direction * moveOutSpeed * Time.deltaTime;
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// Arr�te l'animation d'oscillation.
    /// </summary>
    public void StopRotation()
    {
        isRotating = false;
    }
}
