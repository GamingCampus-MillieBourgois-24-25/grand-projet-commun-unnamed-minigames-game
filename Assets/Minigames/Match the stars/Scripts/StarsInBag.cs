using Axoloop.Global;
using AxoLoop.Minigames.MatchTheStars;
using UnityEngine;

public class StarsInBag : SingletonMB<StarsInBag>
{
    [SerializeField] private StarSlot[] stars; // les 9 boutons à assigner

    public void SetNineStarsInOrder()
    {
        if (stars.Length < 9)
        {
            Debug.LogWarning("Pas assez de boutons !");
            return;
        }

        Sprite[] allColoredStars = MatchTheStarsMinigameData.ColoredStarsSpritesList;

        if (allColoredStars.Length < 9)
        {
            Debug.LogWarning("Pas assez de sprites dans la liste !");
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            stars[i].SetStar(allColoredStars[i]);
        }
    }

    public void RaplaceTakenStar(Sprite starSprite)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i].IsEmpty)
            {
                stars[i].SetStar(starSprite);
                break;
            }
        }
    }
}