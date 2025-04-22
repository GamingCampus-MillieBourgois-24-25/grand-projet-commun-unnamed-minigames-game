using System;
using Axoloop.Global;
using UnityEngine;


namespace AxoLoop.Minigames.FightTheFoes
{
    public class Axo : SingletonMB<Axo>
    {
        [SerializeField] protected Sprite AliveSprite;


        public void Die(Action callBack)
        {
            Destroy(Instance.gameObject);
            callBack?.Invoke();
        }

        public void PlayAttack(FoeType targetType, Action callBack)
        {
            switch (targetType)
            {
                case FoeType.Liquid: 
                    break;
                case FoeType.Food: 
                    break;
                case FoeType.Wind: 
                    break;
                case FoeType.Fire: 
                    break;
            }
        }
    }
}
