using System;
using System.Collections;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class Slime : Foe
    {
        FoeType FoeType = FoeType.Liquid;

        [SerializeField] Sprite DeadSprite;

        protected override void DieAnimation(Action callBack)
        {
            StartCoroutine(PlayDeathAnimation(() => base.DieAnimation(callBack)));

        }

        IEnumerator PlayDeathAnimation(Action callBack)
        {
            spriteRenderer.sprite = DeadSprite;
            yield return new WaitForSeconds(2);
            callBack.Invoke();
        }
    }
}
