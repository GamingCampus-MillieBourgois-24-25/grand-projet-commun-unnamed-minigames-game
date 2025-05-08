using System.Collections.Generic;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public static class FoeFightingUtils
    {
        public static System.Action ButtonsEnter;
        public static System.Action ButtonsExit;
        public static System.Action ButtonsHit;

        public static FTFDifficultyMeter SetDifficulty(MinigameDifficultyLevel difficulty)
        {
            FTFDifficultyMeter difficulty1;
            switch (difficulty)
            {
                case MinigameDifficultyLevel.FirstTime:
                case MinigameDifficultyLevel.VeryEasy:
                    difficulty1 = FTFDifficultyMeter.Easy; break;
                case MinigameDifficultyLevel.Easy:
                case MinigameDifficultyLevel.Medium:
                    difficulty1 = FTFDifficultyMeter.Normal; break;
                case MinigameDifficultyLevel.Hard:
                case MinigameDifficultyLevel.VeryHard:
                case MinigameDifficultyLevel.Impossible:
                default: difficulty1 = FTFDifficultyMeter.Hard; break;
            }
            return difficulty1;
        }

        /// <summary>
        /// Créer une liste d'ennemis à affronter
        /// </summary>
        public static List<Foe> GenerateEnnemies(List<Foe> foes, int nbEnemies, FTFDifficultyMeter difficulty)
        {
            if (foes.Count == 0)
            {
                Debug.LogError("Liste d'ennemis vide ou introuvable");
                return null;
            }

            //Créer un dictionnaire de poids pour une sélection pondérée
            Dictionary<Foe, int> foesWeights = RandomUtils.CreateWeightsDictionary(foes, 1);
            List<Foe> gameFoes = new List<Foe>();
            
            // Forcer le premier ennemi à être de type feu en difficulté facile (feu est le plus intuitif à combattre)
            if (difficulty == FTFDifficultyMeter.Easy)
            {
                var fireFoe = foes.Find(x => x.FoeType == FoeType.Fire);
                gameFoes.Add(fireFoe);
                foesWeights[fireFoe] = 0; // Set weight to 0 to prevent reselection
                nbEnemies--;
            }

            for (int i = 0; i < nbEnemies; i++)
            {
                Foe selectedFoe = RandomUtils.SelectWeightedRandom(foesWeights);
                gameFoes.Add(selectedFoe);
                foesWeights[selectedFoe] = 0; // Set weight to 0 to prevent reselection
            }
            return gameFoes;
        }


        public static List<FoeType> ShuffleAttacks(FTFDifficultyMeter shuffleMode, List<AttackObject> attackList, int turn)
        {
            if (attackList.Count == 0)
            {
                Debug.LogError("Liste d'attaques vide ou introuvable");
                return null;
            }
            
            FoeType currentFoeType = FoeFightMinigameData.CurrentFoe.FoeType;

            // Créer un dictionnaire de poids pour une sélection pondérée
            float weightModifier = (shuffleMode == FTFDifficultyMeter.Hard) ? 0.5f : 1f;
            Dictionary<FoeType, int> attackWeights = new Dictionary<FoeType, int>();
            foreach (var attack in attackList)
            {
                attackWeights[attack.attackType] = (attack.attackType == currentFoeType)    // rendre l'attaque efficace plus probable selon le tour actuel
                                                    ? Mathf.RoundToInt(turn*10*weightModifier) 
                                                    : 10;
            }

            // sélectionner deux attaques aléatoires
            List<FoeType> selectedAttacks = new();
            for (int i = 0; i < 2; i++)
            {
                FoeType pickedAttack = RandomUtils.SelectWeightedRandom(attackWeights);  
                selectedAttacks.Add(pickedAttack);
                attackWeights[pickedAttack] = 0; // Set weight to 0 to prevent reselection
            }

            // Garentir la présence de la bonne attaque en difficulté facile
            if (shuffleMode == FTFDifficultyMeter.Easy)
            {
                if (!selectedAttacks.Contains(currentFoeType))
                {
                    int index = (Random.Range(0, 2) == 0) ? 0 : 1;
                    selectedAttacks[index] = currentFoeType;
                }
            }

            return selectedAttacks;
        }
    }       
}

