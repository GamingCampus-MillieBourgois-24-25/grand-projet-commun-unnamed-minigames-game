using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// G�re l'EventSystem et l'AudioListener pour �viter les conflits entre plusieurs sc�nes.
/// </summary>
public class GlobalSystemManager : MonoBehaviour
{
    private EventSystem _eventSystem;
    private AudioListener _audioListener;

    void Awake()
    {
        _eventSystem = GetComponentInChildren<EventSystem>();
        _audioListener = GetComponentInChildren<AudioListener>();

        // D�sactiver les composants si la sc�ne est charg�e en mode additif
        if (IsSceneAdditive())
        {
            if(_eventSystem)
                _eventSystem.enabled = false;

            if (_audioListener)
                _audioListener.enabled = false;
        }
    }

    private bool IsSceneAdditive()
    {
        // Si plus d'une sc�ne est charg�e, on consid�re que cette sc�ne est additive
        return UnityEngine.SceneManagement.SceneManager.sceneCount > 1;
    }
}