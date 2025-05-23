using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class DropCoin : MonoBehaviour
{
    [SerializeField] private RectTransform _imagePrefab;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private Vector2 _spawnPosition = new Vector2(0f, 1250f);
    [SerializeField] private float _endY = -260f; 
    private float _duration = 1.5f;
    private int _coinCount; 
    private float _spawnInterval = 0.05f;

    void Start()
    {
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        _coinCount = ScoreManager.Instance.GetAmount();
        if (_coinCount > 30) _coinCount = 30;
        for (int i = 0; i < _coinCount; i++)
        {
            if (i > 0)
                yield return new WaitForSeconds(_spawnInterval);

            RectTransform coin = Instantiate(_imagePrefab, _parent);
            coin.anchoredPosition = _spawnPosition;

            coin.DOAnchorPosY(_endY, _duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(coin.gameObject));
        }
    }
}