using Assets.SimpleLocalization.Scripts;
using AxoLoop.Minigames.MatchTheStars;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] string localizationKey;
    Action callback;
    // Start is called before the first frame update

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.Localize(localizationKey);
    }

    public void AnimationFinished()
    {
        callback?.Invoke();
        StartCoroutine(Delay(() => gameObject.SetActive(false)));
    }
    IEnumerator Delay(System.Action action)
    {
        yield return new WaitForSeconds(1f);
        action?.Invoke();
    }
    public void Enable(Action _callback)
    {
        callback = _callback;
        gameObject.SetActive(true);
    }
    //private void OnEnable()
    //{
    //    Enter();
    //}

    //private void Enter()
    //{
    //    textRect.DOAnchorPosX(-500, 1f)
    //        .From()
    //        .SetEase(curve)
    //        //.SetEase(Ease.OutBack)
    //        .OnComplete(Exit);
    //}

    //private void Exit()
    //{
    //    textRect.DOAnchorPosX(500, 2.5f)
    //        .SetEase(Ease.InBack)
    //        .OnComplete(() => gameObject.SetActive(false));
    //}
}
