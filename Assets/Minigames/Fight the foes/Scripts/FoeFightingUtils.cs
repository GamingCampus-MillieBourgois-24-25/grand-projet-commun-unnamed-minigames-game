using System.Collections.Generic;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public static class FoeFightingUtils
    {

        public static DifficultyMeter SetDifficulty(MinigameDifficultyLevel difficulty)
        {
            DifficultyMeter difficulty1;
            switch (difficulty)
            {
                case MinigameDifficultyLevel.FirstTime:
                case MinigameDifficultyLevel.VeryEasy:
                case MinigameDifficultyLevel.Easy:
                    difficulty1 = DifficultyMeter.Easy; break;
                case MinigameDifficultyLevel.Medium:
                case MinigameDifficultyLevel.Hard:
                case MinigameDifficultyLevel.VeryHard:
                case MinigameDifficultyLevel.Impossible:
                    difficulty1 = DifficultyMeter.Normal; break;
                default: difficulty1 = DifficultyMeter.Normal; break;
            }
            return difficulty1;
        }

        public static List<Foe> GenerateEnnemies(List<Foe> foes, int nbEnemies)
        {
            List<Foe> gameFoes = new List<Foe>();
            if (foes.Count != 0)
            {
                for (int i = 0; i < nbEnemies; i++)
                {
                    int foePicked = Random.Range(0, foes.Count);
                    gameFoes.Add(foes[foePicked]);
                }
            }
            else
            {
                Debug.LogError("Liste d'ennemis vide ou introuvable");
                return null;
            }
            return gameFoes;
        }

        public static bool EnnemySpawn(List<Foe> gamefoes)
        {
            if (gamefoes.Count != 0)
            {
                FoeFightMinigameData.CurrentFoe = gamefoes[0];
                return true;
            }
            return false;
        }


        public static List<FoeType> ShuffleAttacks(DifficultyMeter shuffleMode, List<IAttack> attackList)
        {
            List<FoeType> attacks = new();
            FoeType foeType = FoeFightMinigameData.CurrentFoe.FoeType;


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


            if (shuffleMode == DifficultyMeter.Easy)
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
}
