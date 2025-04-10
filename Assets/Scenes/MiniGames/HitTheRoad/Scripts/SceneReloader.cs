using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileSceneReloader : MonoBehaviour
{
    void Update()
    {
        // Recharge la sc�ne actuelle quand on tape sur l'�cran ou clique sur la souris
        if (Input.touchCount > 3 || Input.GetMouseButtonDown(0))
        {
            ReloadScene();
        }
    }

    void ReloadScene()
    {
        // R�cup�re la sc�ne actuelle
        Scene currentScene = SceneManager.GetActiveScene();
        // Recharge la sc�ne
        SceneManager.LoadScene(currentScene.name);
    }
}
