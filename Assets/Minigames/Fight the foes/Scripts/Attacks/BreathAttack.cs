
using System;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class BreathAttack : MonoBehaviour, IAttack
    {
        public FoeType attackType => FoeType.Wind;

        public void PlayAttack(Action<FoeType> hitCallBack)
        {
            //start attack
            //...
            //Attack hit
            hitCallBack.Invoke(attackType);
            //..
            // Attack end
            return;
        }
    }
}