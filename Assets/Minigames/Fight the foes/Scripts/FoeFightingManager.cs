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
            var attackObject = FoeFightMinigameData.AttackObjectList.FirstOrDefault(item => item.attackType == type);

            blocking = false;
            if (attackObject == null)
            {
                blocking = true;
            }

            attackObject.attack.PlayAttack(OnAttackHit);
        }

        void OnAttackHit(FoeType attackType)
        {
            foeType = FoeFightMinigameData.CurrentFoe.FoeType;
            if (attackType == foeType)
            {
                // réaction à l'attaque efficace
                FoeFightMinigameData.CurrentFoe.DieAnimation(() => FoeFightingController.Instance.CurrentFoeKilled());
            }
            else
            {
                // réaction aux attaques inéfficaces
                FoeFightMinigameData.CurrentFoe.TankAnimation(() => FoeFightingController.Instance.FoeTurn(blocking));
            }
        }

    }
    public enum DifficultyMeter
    {
        Easy,
        Normal
    }
}
