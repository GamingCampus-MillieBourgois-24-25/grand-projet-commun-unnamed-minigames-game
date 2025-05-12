using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Axoloop.Global;
using AxoLoop.Minigames.FightTheFoes;
public class RageBar : SingletonMB<RageBar>
{
    [Header("Configuration")]
    [SerializeField] private float fullSize = 100f;
    [SerializeField] private float fillSpeed = 8f;
    [SerializeField] private float rageReduction = 28f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float followSliderDelay = 0.8f;
    [SerializeField] private float followSliderDuration = 0.5f;
    [SerializeField] private float skullPulseSpeed = 5f;

    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private Slider followSlider;
    [SerializeField] private RectTransform skull;

    private float actualFill = 0f;
    private bool stopFill = false;
    private Tween fillTween;
    private Tween followSliderTween;
    private Tween skullTween;
    private float phase = 0f; // Nouvelle variable pour la phase de l'oscillation

    private void Start()
    {
        slider.value = 0;
        followSlider.value = 0;

        AnimateSkull();
        StartFill();
    }

    protected override void OnDestroy()
    {
        KillAllTweens();
        base.OnDestroy();
    }

    public void StopFill()
    {
        stopFill = true;
        fillTween?.Pause();
    }

    public void ResumeFill()
    {
        stopFill = false;
        StartFill();
    }

    private void StartFill()
    {
        fillTween?.Kill();

        if (stopFill) return;

        float remainingFill = fullSize - actualFill;
        float fillDuration = remainingFill / fillSpeed;

        fillTween = DOTween.To(
            () => actualFill,
            value => {
                actualFill = value;
                slider.value = actualFill / fullSize;
            },
            fullSize,
            fillDuration
        ).SetEase(Ease.Linear)
        .OnComplete(() => {
            StopFill();
            FoeFightMinigameData.LockedAttack = true;
            FoeFightingUtils.ButtonsExit.Invoke();
            FoeFightingController.Instance.FoeTurn();
        });
    }

    public void ReduceRage()
    {
        float oldValue = actualFill;
        actualFill = Mathf.Clamp(actualFill - rageReduction, 0, fullSize);
        slider.value = actualFill / fullSize;

        AnimateFollowSlider(oldValue);
    }

    private void AnimateFollowSlider(float fromValue)
    {
        followSliderTween?.Kill();

        followSlider.value = fromValue / fullSize;
        followSliderTween = DOTween.Sequence()
            .AppendInterval(followSliderDelay)
            .Append(
                DOTween.To(
                    () => fromValue / fullSize,
                    value => followSlider.value = value,
                    (actualFill / fullSize) - 0.1f,
                    followSliderDuration
                ).SetEase(Ease.InOutQuad)
            );
    }

    private void AnimateSkull()
    {
        skullTween?.Kill();

        skullTween = DOTween.Sequence()
            .SetLoops(-1, LoopType.Restart)
            .OnUpdate(() => {
                float baseFrequency = 1f;
                float frequencyMultiplier = slider.value;

                float pulseFactor = baseFrequency * frequencyMultiplier * skullPulseSpeed;

                // Incrémenter la phase en fonction de la fréquence et du temps écoulé
                phase += pulseFactor * Time.deltaTime;
                phase %= (2 * Mathf.PI); // Normaliser la phase pour éviter les grandes valeurs

                float influence = (Mathf.Sin(phase) + 1.5f) / 4f;

                float scale = 0.3f + (influence * slider.value * maxScale);

                skull.localScale = new Vector3(scale, scale, scale);
            });
    }

    public void SetFillSpeed(FTFDifficultyMeter difficulty)
    {
        switch (difficulty)
        {
            case FTFDifficultyMeter.Easy:
                fillSpeed = 5;
                rageReduction = 28;
                break;
            case FTFDifficultyMeter.Normal:
                fillSpeed = Random.Range(6, 10);
                rageReduction = 22;
                break;
            case FTFDifficultyMeter.Hard:
                fillSpeed = Random.Range(10, 16);
                rageReduction = 19;
                break;
        }

        if (!stopFill)
        {
            StopFill();
            ResumeFill();
        }
    }

    private void KillAllTweens()
    {
        fillTween?.Kill();
        followSliderTween?.Kill();
        skullTween?.Kill();

        fillTween = null;
        followSliderTween = null;
        skullTween = null;
    }
}