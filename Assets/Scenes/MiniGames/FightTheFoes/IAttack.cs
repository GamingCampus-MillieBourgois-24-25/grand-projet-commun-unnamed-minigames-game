public interface IAttack
{
    AttackType Type { get; }
    void PlayAttack(System.Action onHit);
}
