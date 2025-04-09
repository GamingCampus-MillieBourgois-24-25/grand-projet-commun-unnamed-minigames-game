using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Gère l'EventSystem et l'AudioListener pour éviter les conflits entre plusieurs scènes.
/// </summary>
public class GlobalSystemManager : MonoBehaviour
{
    private EventSystem _eventSystem;
    private AudioListener _audioListener;

    void Awake()
    {
        _eventSystem = GetComponentInChildren<EventSystem>();
        _audioListener = GetComponentInChildren<AudioListener>();

        // Désactiver les composants si la scène est chargée en mode additif
        if (IsSceneAdditive())
        {
            _eventSystem.enabled = false;
            _audioListener.enabled = false;
        }
    }

    private bool IsSceneAdditive()
    {
        // Si plus d'une scène est chargée, on considère que cette scène est additive
        return UnityEngine.SceneManagement.SceneManager.sceneCount > 1;
    }
}