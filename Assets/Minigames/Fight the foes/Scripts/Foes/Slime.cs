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
            //Report the callback and let the frog make a return animation before. Or play the callback after the attack if axo isn't blocking and is dead. 
            if(FoeFightMinigameData.IsBlocking)
            {
                animator.SetBool("AttackReturn", true);
                reportedCallback = callBack;
                base.AttackAnimation(ReportedCallback);
            }
            else
            {
                animator.SetBool("AttackReturn", false);
                base.AttackAnimation(callBack);
            }
        }

        void ReportedCallback()
        {
            currentCallback = reportedCallback;
        }

    }
}