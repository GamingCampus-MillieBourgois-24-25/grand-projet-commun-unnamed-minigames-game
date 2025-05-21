using System.Linq;
using Axoloop.Global;
using UnityEngine;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingManager : SingletonMB<FoeFightingManager>
    {

        FoeType foeType;


        [SerializeField] GameObject[] GreenVariant;
        [SerializeField] GameObject[] GreyVariant;
        [SerializeField] GameObject logo1;
        [SerializeField] GameObject logo2;

        public void SetEnvironmentVariant(bool grey)
        {
            var variant = grey ? GreyVariant : GreenVariant;
            foreach (var item in variant)
            {
                item.SetActive(true);
            }
        }

        public void SetLogoVariant(bool first)
        {
            var variant = first ? logo1 : logo2;
            variant.SetActive(true);
        }


        public void PlayAttack(FoeType type)
        {
            FoeFightMinigameData.LockedAttack = true;

            var attackObject = FoeFightMinigameData.AttackObjectList.FirstOrDefault(item => item.attackType == type);

            FoeFightMinigameData.Axo.PlayAttack(attackObject.attackAnimation, () => OnAttackHit(attackObject.attackType));
        }

        public void PlayBlock()
        {
            FoeFightMinigameData.IsBlocking = true;
            FoeFightingController.Instance.FoeTurn();
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
                FoeFightMinigameData.CurrentFoe.TankAnimation(FoeFightingController.Instance.FoeTurn);
            }
        }

    }
    public enum FTFDifficultyMeter
    {
        Easy,
        Normal, 
        Hard
    }
}
