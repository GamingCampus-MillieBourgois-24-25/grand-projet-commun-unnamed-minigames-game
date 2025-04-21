using System;
using Axoloop.Global;
using UnityEngine;


namespace AxoLoop.Minigames.FightTheFoes
{
    public class Axo : SingletonMB<Axo>
    {
        [SerializeField] protected Sprite AliveSprite;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Die(Action callBack)
        {
            Destroy(Instance.gameObject);
            callBack?.Invoke();
        }
    }
}
