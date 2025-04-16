using System;
using System.Collections;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class Firefox : Foe
    {
        FoeType FoeType = FoeType.Fire;

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
            float duration = 1f;
            float elapsedTime = 0f;
            while(elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                DeadSprite.color = new Vector4(1, 1, 1, elapsedTime/duration);
                yield return null;
            }
            AliveSprite.enabled = false;
            yield return new WaitForSeconds(1f);
            callBack.Invoke();
        }
    }
}
