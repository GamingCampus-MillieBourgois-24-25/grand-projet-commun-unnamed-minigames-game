using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public static class FoeFightingUtils
{
    public static List<FoeType> ShuffleAttacks(DifficultyMeter shuffleMode, List<IAttack> attackList)
    {
        List<FoeType> attacks = new();
        FoeType foeType = FoeFightMinigameData.CurrentFoe.FoeType;


        DifficultyMeter difficultyParam = DifficultyMeter.Easy;

        for (int i = 0; i < 2; i++)
        {
            int RandomNumberPicked = Random.Range(0, attackList.Count);
            if (i == 1)
            {
                while (attacks.Contains(attackList[RandomNumberPicked].attackType))
                {
                    RandomNumberPicked = Random.Range(0, attackList.Count);
                }
            }
            attacks.Add(attackList[RandomNumberPicked].attackType);
        }


        if (difficultyParam == DifficultyMeter.Easy)
        {

            for (int i = 0; i < attacks.Count; i++)
            {
                if (!attacks.Contains(foeType))
                {
                    int index = (Random.Range(0, 2) == 0) ? 0 : 1;
                    attacks[index] = foeType;
                }
            }
        }

        return attacks;
    }
}
