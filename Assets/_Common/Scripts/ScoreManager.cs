using Axoloop.Global;
using UnityEngine;

public class ScoreManager : SingletonMB<ScoreManager> // SingletonMB is a custom class that inherits from MonoBehaviour
{

    [SerializeField] private int totalScore = 0;
    [SerializeField] private int currentScore = 0;
    private int _amount;

    protected override void Awake()
    {
        base.Awake();
        totalScore = PlayerPrefs.GetInt("totalscore");
    }
    
    public int AddTotalScore(int amount)
    {
        totalScore += amount;
        PlayerPrefs.SetInt("totalscore",totalScore);
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
        _amount = amount;
        return currentScore;
    }
    
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    public int GetAmount()
    {
        return _amount;
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }
}

