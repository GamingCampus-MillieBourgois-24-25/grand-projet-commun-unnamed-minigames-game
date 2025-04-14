using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}

    [SerializeField] private int score = 100;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        //Test
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddScore(10);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
    
    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
    }
}

