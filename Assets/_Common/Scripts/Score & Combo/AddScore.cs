using UnityEngine;

public class AddScore : MonoBehaviour
{
    private int _score;
    private int NewScore;
    private int _combo;
    void Start()
    {
        CalulScore();
    }

    private int CalulScore()
    {
        _combo = ComboManager.Instance.GetCombo();
        _score = ScoreManager.Instance.AddCurrentScore(_combo);
        return _score;
    }
}
