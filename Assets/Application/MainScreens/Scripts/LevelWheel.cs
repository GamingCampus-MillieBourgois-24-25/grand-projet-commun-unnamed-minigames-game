using DG.Tweening;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

public class LevelWheel : MonoBehaviour
{
    [SerializeField] private GameObject wheel;
    [SerializeField] private float rotationDuration = 5f;
    [SerializeField] private Ease easeType = Ease.OutBack;
    [SerializeField] private TextMeshProUGUI _textScore;
    [SerializeField] bool fromZero = true;
    
    private float _lastTargetRotation = -1f;
    void Start()
    {
        int score = ScoreManager.Instance.GetTotalScore();
        float targetRotation = GetTargetRotationFromScore(score);

        if (!fromZero)
        {
            _lastTargetRotation = GetTargetRotationFromScore(score - ComboManager.Instance.GetCombo());
            wheel.transform.rotation = Quaternion.Euler(0, 0, _lastTargetRotation);
        }

        if (!Mathf.Approximately(targetRotation, _lastTargetRotation))
        {
            if (score != 0)
            {
                AnimateWheel(targetRotation);
                _lastTargetRotation = targetRotation;
            }
        }
        
        if(_textScore != null)
            _textScore.text = score.ToString("n0");
    }

    private void AnimateWheel(float rotationZ)
    {
        wheel.transform
            .DORotate(new Vector3(0, 0, rotationZ), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(easeType);
    }


    private float GetTargetRotationFromScore(float score)
    {
        float[] thresholds = { 0, 5, 10, 20, 50, 100, 600 };
        float[] angles = { 0, 60, 120, 180, 240, 300, 360 };

        for (int i = 0; i < thresholds.Length - 1; i++)
        {
            if (score >= thresholds[i] && score < thresholds[i + 1])
            {
                float t = (score - thresholds[i]) / (thresholds[i + 1] - thresholds[i]);
                return Mathf.Lerp(angles[i], angles[i + 1], t);
            }
        }

        return 360f;
    }
}