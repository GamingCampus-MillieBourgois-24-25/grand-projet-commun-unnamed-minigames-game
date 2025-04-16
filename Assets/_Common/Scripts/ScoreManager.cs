using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}

    [SerializeField] private int totalScore = 0;
    [SerializeField] private int currentScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public int AddTotalScore(int amount)
    {
        totalScore += amount;
        return totalScore;
    }
    
    public int GetTotalScore()
    {
        return totalScore;
    }

    public void ResetTotalScore()
    {
        totalScore = 0;
    }
    
    public int AddCurrentScore(int amount)
    {
        currentScore += amount;
        return currentScore;
    }
    
    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }
}

