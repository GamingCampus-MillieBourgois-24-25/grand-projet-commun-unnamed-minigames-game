using UnityEngine;

public class AddScoreToTotal : MonoBehaviour
{
    private int _currentScore;
    private void Start()
    {
        CalulTotalScore();
    }

    private void CalulTotalScore()
    {
        _currentScore = ScoreManager.Instance.GetCurrentScore();
        ScoreManager.Instance.AddTotalScore(_currentScore);
    }
}
