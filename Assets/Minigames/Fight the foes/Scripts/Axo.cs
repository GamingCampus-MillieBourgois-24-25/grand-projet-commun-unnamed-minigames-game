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
            if(FoeFightMinigameData.IsBlocking)
            {
                animator.Play("Tank", -1, 0);
                FoeFightingUtils.ButtonsHit?.Invoke();
                return;
            }

            switch (type)
            {
                case FoeType.Liquid:
                    animator.Play("DieStomp", -1, 0);
                    break;
                case FoeType.Fire:
                    animator.Play("DieBurn", -1, 0);
                    break;
                case FoeType.Food:
                    animator.Play("DieShatter", -1, 0);
                    break;
                case FoeType.Wind:
                    animator.Play("DieEject", -1, 0);
                    break;
            }
        }
    }
}
