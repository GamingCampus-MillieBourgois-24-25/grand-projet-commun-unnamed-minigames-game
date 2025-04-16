using UnityEngine;
using DG.Tweening;

public class CloseSceneAnim : MonoBehaviour
{
    [SerializeField] private RectTransform _panel;
    [SerializeField] private float _delay = 3f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _endX = 0f;
    [SerializeField] private Ease _ease = Ease.InOutQuad;

    void Start()
    {
        // Attendre _delay secondes avant de fermer
        DOVirtual.DelayedCall(_delay, () =>
        {
            _panel.DOAnchorPosX(_endX, _duration).SetEase(_ease);
        });
    }
}