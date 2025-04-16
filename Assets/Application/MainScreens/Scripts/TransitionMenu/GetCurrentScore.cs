using UnityEngine;
using TMPro;
public class GetCurrentScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentTextScore;
    void Start()
    {
        int _currentScore = ScoreManager.Instance.GetCurrentScore();
        _currentTextScore.text = _currentScore.ToString("n0");
    }
}
