using System;
using Axoloop.Global;
using UnityEngine;


namespace AxoLoop.Minigames.FightTheFoes
{
    public class Axo : SingletonMB<Axo>
    {
        [SerializeField] protected Sprite AliveSprite;
        private SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = AliveSprite;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Spawn(Action callBack)
        {
            callBack?.Invoke();
        }

        public void Die(Action callBack)
        {
            callBack?.Invoke();
        }
    }
}
