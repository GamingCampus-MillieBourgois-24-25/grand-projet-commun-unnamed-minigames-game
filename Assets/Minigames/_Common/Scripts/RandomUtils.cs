
using AxoLoop.Minigames.FightTheFoes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomUtils
{

    public static Dictionary<T, int> CreateWeightsDictionary<T>(List<T> list, int baseWeight)
    {
        return list.ToDictionary(f => f, f => baseWeight);
    }


    /// <summary>
    /// Select a random item from a dictionary based on its weights.
    /// </summary>
    /// <typeparam name="T">Key of your dictionary</typeparam>
    public static T SelectWeightedRandom<T>(Dictionary<T, int> weights)
    {
        int totalWeight = 0;
        foreach (var weight in weights.Values)
            totalWeight += weight;

        int randomWeight = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var pair in weights)
        {
            cumulativeWeight += pair.Value;
            if (randomWeight < cumulativeWeight)
            {
                return pair.Key;
            }
        }

        Debug.LogError("No item selected, check weights");
        throw new System.Exception("No item selected, check weights");
    }
}

