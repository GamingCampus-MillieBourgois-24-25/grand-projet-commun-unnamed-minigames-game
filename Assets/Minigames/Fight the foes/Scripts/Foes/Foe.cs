using System;
using UnityEngine;


namespace AxoLoop.Minigames.FightTheFoes
{
    public enum FoeType
    {
        Liquid,
        Fire,
        Food,
        Wind, 
    }

    [RequireComponent(typeof(Animator))]
    public class Foe : MonoBehaviour
    {
        public string Name { get; private set; }

        public virtual FoeType FoeType { get; }

        protected Animator animator;

        protected Action currentCallback;
        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
        }



        public void AnimationFinished()
        {
            currentCallback?.Invoke();
            currentCallback = null;
        }


        public virtual void AttackAnimation(Action callBack)
        {
            currentCallback = callBack;
            callBack?.Invoke();
        }

        public virtual void DieAnimation(Action callBack)
        {
            currentCallback = callBack;
            animator.SetTrigger("DieTrigger");
        }

        public virtual void TankAnimation(Action callBack)
        {
            currentCallback = callBack;
            animator.SetTrigger("TankTrigger");
        }

    }
}
