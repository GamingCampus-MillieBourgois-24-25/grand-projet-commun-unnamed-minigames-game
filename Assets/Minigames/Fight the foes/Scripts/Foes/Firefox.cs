using System;
using System.Collections;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class Firefox : Foe
    {
        FoeType FoeType = FoeType.Fire;

        [SerializeField] Sprite DeadSprite;

        protected override void Die(Action callBack)
        {
            StartCoroutine(PlayDeathAnimation(() => base.Die(callBack)));

        }

        IEnumerator PlayDeathAnimation(Action callBack)
        {
            spriteRenderer.sprite = DeadSprite;
            yield return new WaitForSeconds(2);
            callBack.Invoke();
        }
    }
}
