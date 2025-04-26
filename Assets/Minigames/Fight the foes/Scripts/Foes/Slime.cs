using System;
using System.Collections;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class Slime : Foe
    {
        public override FoeType FoeType => FoeType.Liquid;

        public override void AttackAnimation(Action callBack)
        {
            animator.SetBool("AttackReturn", FoeFightMinigameData.IsBlocking);
            base.AttackAnimation(callBack);
        }
    }
}