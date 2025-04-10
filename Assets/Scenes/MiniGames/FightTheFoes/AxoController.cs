using System.Collections.Generic;
using System;
using UnityEngine;

public class AxoController : MonoBehaviour
{
    public GameObject waterPrefab, strawPrefab, bitePrefab, windPrefab;

    public void ShowAttackOptions(List<AttackType> options, Action<AttackType> onClick)
    {
        // Crée et affiche les boutons d’attaque (voir UI plus bas)
    }

    public IAttack GetAttackInstance(AttackType type)
    {
        GameObject prefab = type switch
        {
            AttackType.Water => waterPrefab,
            AttackType.Straw => strawPrefab,
            AttackType.Bite => bitePrefab,
            AttackType.Wind => windPrefab,
            _ => null
        };
        return Instantiate(prefab).GetComponent<IAttack>();
    }

    public void Die()
    {
        Debug.Log("Axo est mort !");
        // Animation + lock input
    }
}
