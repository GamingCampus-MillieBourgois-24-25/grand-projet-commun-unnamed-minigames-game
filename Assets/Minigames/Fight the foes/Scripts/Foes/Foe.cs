using System;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] protected SpritesGroup AliveSprite;

        public virtual FoeType FoeType { get; }

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
