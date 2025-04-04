using UnityEngine;
using UnityEngine.UI;

public class VoxelGameManager : MonoBehaviour
{
    public static VoxelGameManager Instance;
    public RivalBike rivalBike;
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    private float correctLane;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        correctLane = rivalBike.GetFinalLane();
    }

    public bool IsCorrectChoice(float playerX)
    {
        return Mathf.Abs(playerX - correctLane) < 1f;
    }

    public void PlayerWins()
    {
        Debug.Log("Victoire !");
        victoryPanel.SetActive(true);
    }

    public void PlayerFails()
    {
        Debug.Log("Défaite !");
        defeatPanel.SetActive(true);
    }
}
