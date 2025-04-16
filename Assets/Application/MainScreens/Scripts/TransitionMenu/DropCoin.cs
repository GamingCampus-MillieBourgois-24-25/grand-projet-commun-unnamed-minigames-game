using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DropCoin : MonoBehaviour
{
    [SerializeField] private RectTransform _imageToDrop;
    [SerializeField] private float _startY = 1250f;
    [SerializeField] private float _endY = -260f;
    [SerializeField] private float _duration = 3f;

    void Start()
    {
        // Positionner l'image au départ
        Vector2 startPos = _imageToDrop.anchoredPosition;
        startPos.y = _startY;
        _imageToDrop.anchoredPosition = startPos;

        // Lancer l'animation vers la position de fin
        _imageToDrop.DOAnchorPosY(_endY, _duration);
    }
}