
using System.Collections.Generic;
using System.Linq;
using Axoloop.Global;
using UnityEngine;
using static Foe;

public class FoeFightingManager : SingletonMB<FoeFightingManager>
{

    FoeType foeType = FoeFightMinigameData.CurrentFoe.FoeType;


    public void PlayAttack(FoeType type)
    {
        var attack = FoeFightMinigameData.AttackList.FirstOrDefault(item => item.attackType == type);

        if(attack == null)
        {
            Debug.LogError("Pas d'attaque correspondant au type souhaité");
            return;
        }

        attack.PlayAttack(OnAttackHit);
    }
    
    void OnAttackHit(FoeType attackType)
    {
        if (attackType == foeType)
        {
            // FoeFightMinigameData.CurrentFoe.Die();

        }
        else
        {
            //nothing happen, pokemon turn
        }
    }

}

public enum DifficultyMeter
{
    Easy,
    Normal
}