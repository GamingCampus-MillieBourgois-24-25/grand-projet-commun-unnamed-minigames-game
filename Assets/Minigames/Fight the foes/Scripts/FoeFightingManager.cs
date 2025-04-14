using System.Linq;
using Axoloop.Global;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingManager : SingletonMB<FoeFightingManager>
    {

        FoeType foeType;
        private bool blocking = false;


        public void PlayAttack(FoeType type)
        {
            var attack = FoeFightMinigameData.AttackList.FirstOrDefault(item => item.attackType == type);

            blocking = false;
            if (attack == null)
            {
                blocking = true;
            }

            attack.PlayAttack(OnAttackHit);
        }

        void OnAttackHit(FoeType attackType)
        {
            foeType = FoeFightMinigameData.CurrentFoe.FoeType;
            if (attackType == foeType)
            {
                // réaction à l'attaque efficace
                FoeFightMinigameData.GameFoes.RemoveAt(0);
                FoeFightingController.Instance.NextRound();
            }
            else
            {
                // réaction aux attaques inéfficaces
                FoeFightingController.Instance.FoeTurn(blocking);
            }
        }

    }
    public enum DifficultyMeter
    {
        Easy,
        Normal
    }
}
