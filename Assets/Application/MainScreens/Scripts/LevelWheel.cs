using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

public class LevelWheel : MonoBehaviour
{
    [SerializeField] private float _score;
    [SerializeField] private Image wheel;
    
    [SerializeField] private float rotationDuration = 5f;
    [SerializeField] private Ease easeType = Ease.OutBack;

    private float _lastTargetRotation = -1f;
    
    public float Score
    {
        get => _score;
        set => _score = value;
    }
    void Update()
    {
        float targetRotation = GetTargetRotationFromScore(_score);

        // Si on a changé de palier de score
        if (!Mathf.Approximately(targetRotation, _lastTargetRotation))
        {
            AnimateWheel(targetRotation);
            _lastTargetRotation = targetRotation;
        }
    }

    private void AnimateWheel(float rotationZ)
    {
        wheel.transform
            .DORotate(new Vector3(0, 0, rotationZ), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(easeType);
    }


    private float GetTargetRotationFromScore(float score)
    {
        float[] thresholds = { 0, 100, 200, 300, 400, 500, 600 };
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