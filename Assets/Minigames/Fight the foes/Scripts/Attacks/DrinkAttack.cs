
using System;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class DrinkAttack : MonoBehaviour, IAttack
    {
        public FoeType attackType => FoeType.Liquid;

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