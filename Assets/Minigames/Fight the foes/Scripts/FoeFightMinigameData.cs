using System.Collections.Generic;
using Axoloop.Global;
using UnityEngine;
using UnityEngine.UI;


namespace AxoLoop.Minigames.FightTheFoes
{
    /// <summary>
    /// Singleton qui contient les données de la minijeu
    /// </summary>
    public class FoeFightMinigameData : SingletonMB<FoeFightMinigameData>
    {

        Foe currentFoe;
        List<FoeType> currentAttacks = new List<FoeType>();
        List<Foe> gameFoes = new List<Foe>();
        FTFDifficultyMeter difficulty = FTFDifficultyMeter.Easy;
        int currentTurn;

        /// <summary>
        /// Ennemi actuellement contre Axo
        /// </summary>
        public static Foe CurrentFoe { get => Instance.currentFoe; set => Instance.currentFoe = value; }
        /// <summary>
        /// Attaques actuellement disponibles
        /// </summary>
        public static List<FoeType> CurrentAttacks { get => Instance.currentAttacks; set => Instance.currentAttacks = value; }
        /// <summary>
        /// Ennemis que va affronter Axo dans cette partie
        /// </summary>
        public static List<Foe> GameFoes { get => Instance.gameFoes; set => Instance.gameFoes = value; }
        /// <summary>
        /// Difficulté de la partie
        /// </summary>
        public static FTFDifficultyMeter Difficulty { get => Instance.difficulty; set => Instance.difficulty = value; }
        /// <summary>
        /// Tour actuel
        /// </summary>
        public static int CurrentTurn { get => Instance.currentTurn; set => Instance.currentTurn = value; }



        bool lockedAttack;
        bool isBlocking;
        public static bool LockedAttack { 
            get => Instance.lockedAttack; 
            set {
                Instance.lockedAttack = value;
                Instance.LockButtons(!value);
            }
        }
        public static bool IsBlocking { get => Instance.isBlocking; set => Instance.isBlocking = value; }



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

        [SerializeField] List<Button> gameButtons = new List<Button>();

        public static List<AttackObject> AttackObjectList { get => Instance.attackObjectList; set => Instance.attackObjectList = value; }
        public static List<Foe> FoesList { get => Instance.foesList; set => Instance.foesList = value; }
        public static Axo Axo { get => Instance.axo; set => Instance.axo = value; }



        private void LockButtons(bool state)
        {
            gameButtons.ForEach(button => button.interactable = state);
        }

    }
}
