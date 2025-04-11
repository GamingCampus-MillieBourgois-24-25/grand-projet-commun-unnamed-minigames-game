using System.Linq;
using Axoloop.Global;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingManager : SingletonMB<FoeFightingManager>
    {

        FoeType foeType;


        public void PlayAttack(FoeType type)
        {
            var attack = FoeFightMinigameData.AttackList.FirstOrDefault(item => item.attackType == type);

            if (attack == null)
            {
                Debug.LogError("Pas d'attaque correspondant au type souhaité");
                return;
            }

            attack.PlayAttack(OnAttackHit);
        }

        void OnAttackHit(FoeType attackType)
        {
            foeType = FoeFightMinigameData.CurrentFoe.FoeType;
            if (attackType == foeType)
            {
                FoeFightMinigameData.GameFoes.RemoveAt(0);

            }
            else
            {
                //nothing happen, pokemon turn
            }
        }

    }
    public enum DifficultyMeter
    {
        Easy,
        Normal
    }
}
