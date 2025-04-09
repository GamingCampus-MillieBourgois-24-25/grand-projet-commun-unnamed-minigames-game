
using System;
using UnityEngine;
using static Foe;

public interface IAttack
{
    FoeType attackType { get; }
    public void PlayAttack(Action<FoeType> callBack);
}
