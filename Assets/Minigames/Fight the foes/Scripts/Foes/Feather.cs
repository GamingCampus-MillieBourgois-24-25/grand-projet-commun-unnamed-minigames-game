using System;
using System.Collections;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class Feather : Foe
    {
        public override FoeType FoeType => FoeType.Wind;
        [SerializeField] SpriteRenderer DeadSprite;

        protected override void Start()
        {
            base.Start();
            DeadSprite.color = new Vector4(1, 1, 1, 0);
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