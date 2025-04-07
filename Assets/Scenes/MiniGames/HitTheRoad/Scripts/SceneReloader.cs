using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileSceneReloader : MonoBehaviour
{
    void Update()
    {
        // Recharge la scène actuelle quand on tape sur l'écran ou clique sur la souris
        if (Input.touchCount > 3 || Input.GetMouseButtonDown(0))
        {
            ReloadScene();
        }
    }

    void ReloadScene()
    {
        // Récupère la scène actuelle
        Scene currentScene = SceneManager.GetActiveScene();
        // Recharge la scène
        SceneManager.LoadScene(currentScene.name);
    }
}
