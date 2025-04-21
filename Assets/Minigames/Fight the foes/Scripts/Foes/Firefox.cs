using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class Firefox : Foe
    {
        public override FoeType FoeType => FoeType.Fire;

        [SerializeField] SpritesGroup DeadSprite; 

        protected override void Start()
        {
            base.Start();
            DeadSprite.color = new Vector4(1, 1, 1, 0);
        }

        protected override void DieAnimation(Action callBack)
        {
            

        }


        public void FadeFire()
        {
            StartCoroutine(PlayDeathAnimationCoroutine());
        }
        IEnumerator PlayDeathAnimationCoroutine()
        {
            float duration = 1f;
            float elapsedTime = 0f;
            while(elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                DeadSprite.color = new Vector4(1, 1, 1, elapsedTime/duration);
                yield return null;
            }
            DeadSprite.color = new Vector4(1, 1, 1, 1);
            AliveSprite.gameObject.SetActive(false);
        }
    }
}
