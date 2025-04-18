using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using AxoLoop.Minigames.FightTheFoes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LocalizeTextTMP))]
public class ButtonController : MonoBehaviour
{

    public FoeType buttonType { get; private set; }
    [SerializeField] SpriteRenderer attackIcon;
    [SerializeField] Image attackImage;

    LocalizeTextTMP localizer;

    private void Start()
    {
        localizer = GetComponentInChildren<LocalizeTextTMP>();
        Image attackImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        
    }

    private void DisableButton()
    {

    }

    public void OnClick()
    {
        FoeFightingManager.Instance.PlayAttack(buttonType);
    }

    public void SetButtonData(FoeType typeData)
    {
        var attack =  FoeFightMinigameData.AttackObjectList.FirstOrDefault(item => item.attackType == typeData);
        buttonType = attack.attackType;
        //localizer.LocalizationKey = attack.attackName;
        attackIcon.sprite = attack.attackIcon;
        attackImage.color = attack.attackColor;
    }
}
