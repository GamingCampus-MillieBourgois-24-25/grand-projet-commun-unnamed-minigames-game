using Axoloop.Global;
using System.Collections;
using UnityEngine;

public class ReviveSceneManager : BaseSceneManager<ReviveSceneManager>, ISceneManager
{
    public override string SceneName { get => "MAIN_Revive"; }

    public override SceneLevel SceneLevel { get => SceneLevel.Level2; }

    public override bool AsyncLoading { get => false; }


    [SerializeField] RectTransform rect;

    protected override void DisableScene()
    {
        // désactiver les inputs et toute action potentielle future

    }

    protected override void PlayUnloadTransition()
    {
        StartCoroutine(Transition(0, -1500));
    }

    protected override void PlayLoadTransition()
    {
        StartCoroutine(Transition(-1500, 0));
    }


    IEnumerator Transition(int start, int end)
    {
        rect.anchoredPosition = new Vector2(0, start);
        float time = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            rect.anchoredPosition = new Vector2(0, Mathf.Lerp(start, end, elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}