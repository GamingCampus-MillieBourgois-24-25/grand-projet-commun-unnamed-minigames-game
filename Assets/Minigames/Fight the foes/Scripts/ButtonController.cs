using System.Collections;
using System.Collections.Generic;
using AxoLoop.Minigames.FightTheFoes;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    public FoeType buttonType { get; private set; }

    public void SetButtonType(FoeType buttonTypeParam)
    {
        buttonType = buttonTypeParam;
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
}
