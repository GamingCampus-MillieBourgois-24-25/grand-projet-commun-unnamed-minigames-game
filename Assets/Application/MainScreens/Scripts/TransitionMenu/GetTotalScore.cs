using UnityEngine;
using TMPro;
public class GetTotalScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalTextScore;
    void Start()
    {
        int _totalScore = ScoreManager.Instance.GetTotalScore();
        _totalTextScore.text = _totalScore.ToString("n0");
    }
}
