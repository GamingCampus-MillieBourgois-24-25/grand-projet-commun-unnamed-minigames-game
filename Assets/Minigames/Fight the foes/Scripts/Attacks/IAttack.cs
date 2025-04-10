
using System;

namespace AxoLoop.Minigames.FightTheFoes
{
    public interface IAttack
    {
        FoeType attackType { get; }
        public void PlayAttack(Action<FoeType> callBack);
    }
}