using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// G�re l'EventSystem pour �viter les conflits entre plusieurs sc�nes.
/// </summary>
public class EventSystemManager : MonoBehaviour
{
    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = GetComponent<EventSystem>();
        if (_eventSystem == null)
        {
            Debug.LogError("EventSystemManager doit �tre attach� � un GameObject avec un EventSystem.");
            return;
        }

        // D�sactiver l'EventSystem si la sc�ne est charg�e en mode additif
        if (IsSceneAdditive())
        {
            _eventSystem.enabled = false;
        }
    }

    private bool IsSceneAdditive()
    {
        // Si plus d'une sc�ne est charg�e, on consid�re que cette sc�ne est additive
        return UnityEngine.SceneManagement.SceneManager.sceneCount > 1;
    }
}