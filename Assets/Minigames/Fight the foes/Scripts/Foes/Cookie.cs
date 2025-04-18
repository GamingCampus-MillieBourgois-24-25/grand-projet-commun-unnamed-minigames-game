using System;
using System.Collections;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class Cookie : Foe
    {

        public override FoeType FoeType => FoeType.Food;

        protected override void Start()
        {
            base.Start();
        }

        protected override void DieAnimation(Action callBack)
        {
            StartCoroutine(PlayDeathAnimation(() => base.DieAnimation(callBack)));

        }

        IEnumerator PlayDeathAnimation(Action callBack)
        {
            AliveSprite.enabled = false;
            yield return new WaitForSeconds(1f);
            callBack.Invoke();
        }
    }
}