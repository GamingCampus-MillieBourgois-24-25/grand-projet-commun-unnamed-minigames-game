
using System;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class WaterAttack : MonoBehaviour, IAttack
    {
        public FoeType attackType => FoeType.Fire;

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