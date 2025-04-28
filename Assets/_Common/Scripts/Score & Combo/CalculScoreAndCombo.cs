using Axoloop.Global;
using UnityEngine;

public class CalculScoreAndCombo : SingletonMB<CalculScoreAndCombo> // SingletonMB is a custom class that inherits from MonoBehaviour
{ 
    private int nbMiniGamesWin;
    private int _score;
    private int NewScore;
    private int _combo;
    private int _currentScore;
    
    public void OnMiniGameWon()
    {
        nbMiniGamesWin++;
        if (nbMiniGamesWin == 4)
        {
            ComboManager.Instance.AddCombo(1);
            nbMiniGamesWin = 0;
        }

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
        ScoreManager.Instance.AddTotalScore(_combo);
        
    }

    public int GetCombo()
    {
        return _combo;
    }

}
