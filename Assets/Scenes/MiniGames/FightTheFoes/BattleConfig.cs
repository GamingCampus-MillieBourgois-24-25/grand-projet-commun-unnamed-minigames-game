using UnityEngine;

[CreateAssetMenu(menuName = "Minigame/BattleConfig")]
public class BattleConfig : ScriptableObject
{
    public int numberOfBattles;
    public float threatFillSpeed;
    public float threatReductionPerAction;
    [Range(1, 3)] public int shuffleDifficulty; // 1, 2 ou 3
    public GameObject[] availableEnemies;
}
