using UnityEngine;

public class AddScore : MonoBehaviour
{
    private int _score;
    private int NewScore;
    private int _combo;
    private int _currentScore;
    void Start()
    {
        CalulScore();
        CalculTotalScore();
    }

    private int CalulScore()
    {
        _combo = ComboManager.Instance.GetCombo();
        _score = ScoreManager.Instance.AddCurrentScore(_combo);
        return _score;
    }
    
    private void CalculTotalScore()
    {
        _currentScore = ScoreManager.Instance.GetCurrentScore();
        ScoreManager.Instance.AddTotalScore(_currentScore);
    }
}
