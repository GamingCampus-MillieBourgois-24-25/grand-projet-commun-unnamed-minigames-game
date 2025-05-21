using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Onomatopea : MonoBehaviour
{
    [SerializeField] float scaleA = 0.5f, scaleB = 3.0f, duration = 1.2f;
    [SerializeField] Vector3 shakeStrength = new(0f, 0f, 20f);

    Image uiImage;

    // Start is called before the first frame update
    void Awake()
    {
        uiImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        uiImage.color = Vector4.one;
        Sequence sequence = DOTween.Sequence();

        sequence.Join(uiImage.rectTransform.DOScale(scaleB, duration).From(scaleA).SetEase(Ease.Linear));
        sequence.Join(uiImage.DOFade(0f, duration).SetEase(Ease.InQuart));
        sequence.Join(uiImage.rectTransform.DOShakeRotation(duration, shakeStrength, 10, 1, false, ShakeRandomnessMode.Harmonic));

        sequence.SetAutoKill(true);

        sequence.Play();
    }


}
