using System;
using System.Collections;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class Slime : Foe
    {
        public override FoeType FoeType => FoeType.Liquid;

        Action reportedCallback;

        public override void AttackAnimation(Action callBack)
        {
            reportedCallback = callBack;
            animator.SetBool("AttackReturn", FoeFightMinigameData.IsBlocking);
            base.AttackAnimation(ReportedCallback);
        }

        void ReportedCallback()
        {
            currentCallback = reportedCallback;
        }

    }
}