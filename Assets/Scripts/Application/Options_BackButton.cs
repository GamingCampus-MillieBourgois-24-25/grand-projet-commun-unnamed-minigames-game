using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options_Back : MonoBehaviour
{
    public void CloseScene()
    {
        SceneManager.UnloadSceneAsync(1);
    }
}
