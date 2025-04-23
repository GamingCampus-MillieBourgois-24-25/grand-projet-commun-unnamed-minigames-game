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
        /// <summary>
        /// Ennemi actuellement contre Axo
        /// </summary>
        Foe currentFoe;
        /// <summary>
        /// Attackes actuellement disponibles
        /// </summary>
        List<FoeType> currentAttacks = new List<FoeType>();
        /// <summary>
        /// Ennemis que va affronter Axo dans cette partie
        /// </summary>
        List<Foe> gameFoes = new List<Foe>();

        public static Foe CurrentFoe { get => Instance.currentFoe; set => Instance.currentFoe = value; }
        public static List<FoeType> CurrentAttacks { get => Instance.currentAttacks; set => Instance.currentAttacks = value; }
        public static List<Foe> GameFoes { get => Instance.gameFoes; set => Instance.gameFoes = value; }




        /// <summary>
        /// Liste de toutes les attaques disponibles
        /// </summary>
        [SerializeField] List<AttackObject> attackObjectList = new List<AttackObject>();
        /// <summary>
        /// Liste de tous les ennemis
        /// </summary>
        [SerializeField] List<Foe> foesList = new List<Foe>();
        /// <summary>
        /// Axo
        /// </summary>
        [SerializeField] Axo axo;

        public static List<AttackObject> AttackObjectList { get => Instance.attackObjectList; set => Instance.attackObjectList = value; }
        public static List<Foe> FoesList { get => Instance.foesList; set => Instance.foesList = value; }
        public static Axo Axo { get => Instance.axo; set => Instance.axo = value; }
    }
}
