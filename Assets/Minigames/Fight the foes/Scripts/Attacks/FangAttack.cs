
using System;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FangAttack : MonoBehaviour, IAttack
    {
        public FoeType attackType => FoeType.Food;

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
