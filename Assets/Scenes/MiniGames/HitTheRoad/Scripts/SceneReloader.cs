using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileSceneReloader : MonoBehaviour
{

    void ReloadScene()
    {
        // Récupère la scène actuelle
        Scene currentScene = SceneManager.GetActiveScene();
        // Recharge la scène
        SceneManager.LoadScene(currentScene.name);
    }
}
