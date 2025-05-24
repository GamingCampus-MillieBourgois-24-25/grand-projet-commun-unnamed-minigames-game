using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContinueText : MonoBehaviour
{
    [SerializeField] RectTransform rect;

    bool victory = false;
    Vector2 targetPosition;

    // Start is called before the first frame update
    void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        StartCoroutine(Spawn());
        StartCoroutine(WaitForInput());

        MiniGameManager.Instance?.HideMinigameUI();
    }

    public void Enable(bool _victory)
    {
        victory = _victory;
        gameObject.SetActive(true);
    }

    IEnumerator Spawn()
    {
        var targetPosition = rect.anchoredPosition;
        var startposition = new Vector2(0, -1500);

        var time = 0.7f;
        var elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(startposition, targetPosition, elapsed / time);
            yield return null;
        }
    }

    IEnumerator WaitForInput()
    {
        // On attend que l'utilisateur touche l'écran
        while (!IsUserTapped())
        {
            yield return null; // attend une frame
        }

        MiniGameManager.Instance.MiniGameFinished(victory); // On appelle la fonction de fin de mini-jeu
        gameObject.SetActive(false); // On cache le texte
    }

    private bool IsUserTapped()
    {
        // Fonction qui détecte un "tap" générique
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                return true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            return true; // Permet de tester aussi sur PC
        }
        return false;
    }
}
