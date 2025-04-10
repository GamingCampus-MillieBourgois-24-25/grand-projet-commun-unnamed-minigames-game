using System.Collections.Generic;
using Axoloop.Global;
using UnityEngine;


namespace AxoLoop.Minigames.FightTheFoes
{
    /// <summary>
    /// Singleton qui contient les données de la minijeu
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class FoeFightMinigameData : SingletonMB<FoeFightMinigameData>
    {
        [SerializeField] Foe currentFoe;
        [SerializeField] List<IAttack> attackList = new List<IAttack>();
        [SerializeField] List<Foe> foesList = new List<Foe>();
        [SerializeField] Axo axo;

        public static Foe CurrentFoe { get => Instance.currentFoe; set => Instance.currentFoe = value; }
        public static List<IAttack> AttackList { get => Instance.attackList; set => Instance.attackList = value; }
        public static List<Foe> FoesList { get => Instance.foesList; set => Instance.foesList = value; }
        public static Axo Axo { get => Instance.axo; set => Instance.axo = value; }
    }
}
