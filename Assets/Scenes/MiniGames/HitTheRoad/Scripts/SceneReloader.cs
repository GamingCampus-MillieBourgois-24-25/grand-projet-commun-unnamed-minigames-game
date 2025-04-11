using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileSceneReloader : MonoBehaviour
{

    void ReloadScene()
    {
        // R�cup�re la sc�ne actuelle
        Scene currentScene = SceneManager.GetActiveScene();
        // Recharge la sc�ne
        SceneManager.LoadScene(currentScene.name);
    }
}
