using AxoLoop.Minigames.MatchTheStars;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnStarBox : MonoBehaviour
{
    [SerializeField] float finalPos;
    [SerializeField] float entryPos = -840f;
    
    RectTransform rect;
    Image boxImg;
    Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        finalPos = rect.anchoredPosition.y;

        boxImg = GetComponent<Image>();
        boxImg.enabled = false;
        button = GetComponentInChildren<Button>();
        button.enabled = false;

        MatchTheStarsController.Instance.OnStartSignal += SpawnAnimation;
    }

    void SpawnAnimation()
    {
        boxImg.enabled = true;
        rect.DOAnchorPosY(entryPos, 2f)
            .From()
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                button.enabled = true;
            });
    }


}
