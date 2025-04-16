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

    [RequireComponent(typeof(SpriteRenderer))]
    public class Foe : MonoBehaviour
    {
        public string Name { get; private set; }
        [SerializeField] protected SpriteRenderer AliveSprite;
        [SerializeField] protected FoeType foeType;

        public FoeType FoeType => foeType;

        protected virtual void Start()
        {

        }

        protected virtual void AttackAnimation(Action callBack)
        {
            callBack?.Invoke();
        }

        protected virtual void DieAnimation(Action callBack)
        {
            callBack?.Invoke();
        }

    }
}
