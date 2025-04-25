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

        public void Die(Action callBack)
        {
            Destroy(Instance.gameObject);
            callBack?.Invoke();
        }

        public void PlayAttack(AnimationClip attack, Action callBack)
        {
            attackCallback = callBack;
            animator.Play(attack.name, -1, 0);
        }


        public void AttackTouched()
        {
            attackCallback?.Invoke();
            attackCallback = null;
        }
    }
}
