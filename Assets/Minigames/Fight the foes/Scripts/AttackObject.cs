using AxoLoop.Minigames.FightTheFoes;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "FightTheFoes/Attack")]
public class AttackObject : ScriptableObject
{
    public FoeType attackType;
    public string attackName;
    public Color attackColor;
    public Sprite attackIcon;
}