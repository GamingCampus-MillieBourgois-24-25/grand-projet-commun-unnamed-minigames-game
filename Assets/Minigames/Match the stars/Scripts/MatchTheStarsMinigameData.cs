using System.Collections.Generic;
using Axoloop.Global;
using UnityEngine;
using UnityEngine.UI;


namespace AxoLoop.Minigames.MatchTheStars
{
    /// <summary>
    /// Singleton qui contient les données de la minijeu
    /// </summary>
    public class MatchTheStarsMinigameData : SingletonMB<MatchTheStarsMinigameData>
    {
        [SerializeField] int starsCount = 9; // Nombre d'étoiles à afficher
        [SerializeField] MTSDifficulty difficulty;
        [SerializeField] Sprite emptyStarSprite;
        public static int StarsCount { get => Instance.starsCount; set => Instance.starsCount = value; }
        public static MTSDifficulty Difficulty { get => Instance.difficulty; set => Instance.difficulty = value; }
        public static Sprite EmptyStarSprite { get => Instance.emptyStarSprite; set => Instance.emptyStarSprite = value; } // Sprite de l'étoile vide


        [SerializeField] private Sprite[] availableSpritesList;
        [SerializeField] private Color[] availableColors;
        [SerializeField] private Sprite[] coloredStarsSpritesList;
        [SerializeField] private Image[] crownStarsImages; // les 3 images à assigner


        public static Sprite[] AvailableSpritesList { get => Instance.availableSpritesList; set => Instance.availableSpritesList = value; }
        public static Color[] AvailableColors { get => Instance.availableColors; set => Instance.availableColors = value; }
        public static Sprite[] ColoredStarsSpritesList { get => Instance.coloredStarsSpritesList; set => Instance.coloredStarsSpritesList = value; }
        public static Image[] CrownStarsImages { get => Instance.crownStarsImages; set => Instance.crownStarsImages = value; }


    }

    public enum MTSDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}
