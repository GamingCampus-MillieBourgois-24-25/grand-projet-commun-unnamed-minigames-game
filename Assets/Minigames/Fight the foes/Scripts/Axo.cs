using System;
using Axoloop.Global;
using UnityEngine;


namespace AxoLoop.Minigames.FightTheFoes
{
    [RequireComponent(typeof(Animator))]
    public class Axo : SingletonMB<Axo>
    {
        [SerializeField] protected Sprite AliveSprite;
        private Animator animator;
        private Action attackCallback;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAttack(AnimationClip attack, Action callBack)
        {
            attackCallback = callBack;
            animator.Play(attack.name, -1, 0);
        }


        public void AttackTouched()
        {
            attackCallback?.Invoke();
            FoeFightingUtils.ButtonsHit?.Invoke();
            attackCallback = null;
        }

        public void DieFromFoe(FoeType type)
        {
            switch (type)
            {
                case FoeType.Liquid:
                    animator.SetTrigger("DieStomp");
                    break;
                case FoeType.Fire:
                    animator.SetTrigger("DieBurn");
                    break;
                case FoeType.Food:
                    animator.SetTrigger("DieShatter");
                    break;
                case FoeType.Wind:
                    animator.SetTrigger("DieEject");
                    break;
            }
        }
    }
}
