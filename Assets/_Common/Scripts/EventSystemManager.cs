using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Gère l'EventSystem pour éviter les conflits entre plusieurs scènes.
/// </summary>
public class EventSystemManager : MonoBehaviour
{
    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = GetComponent<EventSystem>();
        if (_eventSystem == null)
        {
            Debug.LogError("EventSystemManager doit être attaché à un GameObject avec un EventSystem.");
            return;
        }

        // Désactiver l'EventSystem si la scène est chargée en mode additif
        if (IsSceneAdditive())
        {
            _eventSystem.enabled = false;
        }
    }

    private bool IsSceneAdditive()
    {
        // Si plus d'une scène est chargée, on considère que cette scène est additive
        return UnityEngine.SceneManagement.SceneManager.sceneCount > 1;
    }
}