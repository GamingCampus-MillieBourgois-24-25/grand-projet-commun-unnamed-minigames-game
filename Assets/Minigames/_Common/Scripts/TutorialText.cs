using Assets.SimpleLocalization.Scripts;
using AxoLoop.Minigames.MatchTheStars;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    IMinigameController minigameController;
    [SerializeField] string localizationKey;
    // Start is called before the first frame update
    void Start()
    {
        minigameController = GameObject.FindGameObjectWithTag("ROOT Controller").GetComponent<IMinigameController>();
        minigameController.OnTutorialSignal += Enable;
        gameObject.SetActive(false);

        GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.Localize(localizationKey);
    }

    public void AnimationFinished()
    {
        minigameController.OnStartSignal?.Invoke();
        StartCoroutine(Delay(() => gameObject.SetActive(false)));
    }
    IEnumerator Delay(System.Action action)
    {
        yield return new WaitForSeconds(1f);
        action?.Invoke();
    }
    private void Enable()
    {
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
