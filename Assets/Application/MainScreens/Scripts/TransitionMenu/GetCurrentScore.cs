using UnityEngine;
using TMPro;
using DG.Tweening; // N'oublie pas d'ajouter ça pour DoTween

public class GetCurrentScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentTextScore;

    void Start()
    {
        int _currentScore = ScoreManager.Instance.GetCurrentScore();
        _currentTextScore.text = _currentScore.ToString("n0");

        AnimateScoreChange();
    }

    private void AnimateScoreChange()
    {
        // On remet l’échelle à 1 au cas où
        _currentTextScore.rectTransform.localScale = Vector3.one;

        // Petit "punch" d’échelle
        _currentTextScore.rectTransform.DOPunchScale(
            new Vector3(0.3f, 0.3f, 0), // Intensité du rebond
            1f, // Durée
            10,   // Vibrations
            1     // Élasticité
        );
    }
}