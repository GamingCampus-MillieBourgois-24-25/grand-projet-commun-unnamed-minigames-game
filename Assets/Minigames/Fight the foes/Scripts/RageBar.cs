using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Axoloop.Global;
using AxoLoop.Minigames.FightTheFoes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class RageBar : SingletonMB<RageBar>
{
    float actualFill = 0;
    float fillSpeed = 8;
    int fullSize = 100;
    float rageReduction = 28;
    bool stopFill = false;
    float maxScale = 1.2f;


    [SerializeField] Slider slider;
    [SerializeField] Slider followSlider;
    [SerializeField] RectTransform skull;

    private void Start()
    {
        StartCoroutine(SkullWave());
    }

    void Update()
    {
        if (!stopFill)
        {
            IncreaseRage();
        }
        
        slider.value = actualFill/fullSize;
        
    }

    public void StopFill()
    {
        stopFill = true;
    }
    public void ResumeFill()
    {
        stopFill = false;
    }

    void IncreaseRage()
    {
        actualFill += Time.deltaTime * fillSpeed;
        if (actualFill >= fullSize)
        {
            StopFill();
            FoeFightMinigameData.LockedAttack = true;
            FoeFightingUtils.ButtonsExit.Invoke();
            FoeFightingController.Instance.FoeTurn();
        }
    }
    
    public void ReduceRage()
    {
        StartCoroutine(SecondSliderFollowUp());
        actualFill -= rageReduction;
    }

    public void SetFillSpeed(DifficultyMeter difficulty)
    {
        switch (difficulty)
        {
            case DifficultyMeter.Easy:
                fillSpeed = 6;
                break;
            case DifficultyMeter.Normal:
                fillSpeed = Random.Range(8, 11);
                break;
            case DifficultyMeter.Hard:
                fillSpeed = Random.Range(11, 16);
                break;
        }
    }


    private IEnumerator SecondSliderFollowUp()
    {
        followSlider.value = actualFill / fullSize;

        yield return new WaitForSeconds(0.8f);

        while(followSlider.value > slider.value-0.05)
        {
            followSlider.value -= Time.deltaTime * 0.5f;
            yield return null;
        }
    }

    private IEnumerator SkullWave()
    {
        float influence;
        while (true)
        {
            float speed = 5f * slider.value;
            influence = (Mathf.Sin(Time.time * speed) + 1.5f) / 6f;
            float scale = 0.3f + (influence * slider.value * maxScale);
            skull.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

    }
}
