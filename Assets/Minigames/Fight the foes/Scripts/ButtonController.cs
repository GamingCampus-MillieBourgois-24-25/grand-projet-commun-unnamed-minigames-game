using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using AxoLoop.Minigames.FightTheFoes;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    // Ctrl + M + O pour déplier toutes les régions
    #region PROPERTIES----------------------------------------------------------------------
    public FoeType buttonType { get; private set; }
    [SerializeField] Image attackIcon;
    [SerializeField] Image attackImage;

    bool selected;
    Animator animator;
    LocalizeTextTMP localizer;

    public Action OnButtonReady;

    #endregion
    #region LIFECYCLE-----------------------------------------------------------------------

    private void Start()
    {
        localizer = GetComponentInChildren<LocalizeTextTMP>();
        animator = GetComponent<Animator>();
        Image attackImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        FoeFightingUtils.ButtonsEnter += PlayEnter;
        FoeFightingUtils.ButtonsExit += PlayExit;
        FoeFightingUtils.ButtonsHit += PlayHit;
    }
    private void OnDisable()
    {
        FoeFightingUtils.ButtonsEnter -= PlayEnter;
        FoeFightingUtils.ButtonsExit -= PlayExit;
        FoeFightingUtils.ButtonsHit -= PlayHit;
    }

    #endregion
    #region METHODS-------------------------------------------------------------------------

    void PlayEnter()
    {
        animator.SetTrigger("Enter");
    }

    void PlayExit()
    {
        if (selected)
        {
            return;
        }
        animator.SetTrigger("Exit");

    }

    void PlaySelect()
    {
        OnButtonReady += ButtonSelected; 
        animator.SetTrigger("Select");
    }

    void PlayHit()
    {
        if (!selected) return;
        selected = false;
        animator.SetTrigger("Hit");
    }

    #endregion
    #region API-----------------------------------------------------------------------------

    public void OnClick()
    {
        selected = true;
        PlaySelect();
        FoeFightingUtils.ButtonsExit?.Invoke();
    }

    public void SetButtonData(FoeType typeData)
    {
        var attack = FoeFightMinigameData.AttackObjectList.FirstOrDefault(item => item.attackType == typeData);
        buttonType = attack.attackType;
        localizer.LocalizationKey = attack.attackName;
        attackIcon.sprite = attack.attackIcon;
        attackImage.color = attack.attackColor;
    }
    public void ButtonReady()
    {
        OnButtonReady?.Invoke();
        OnButtonReady = null;
    }

    public void ButtonSelected()
    {
        OnButtonReady -= ButtonSelected;
        FoeFightingManager.Instance.PlayAttack(buttonType);
    }

    #endregion
    #region COROUTINES----------------------------------------------------------------------

    #endregion





}
