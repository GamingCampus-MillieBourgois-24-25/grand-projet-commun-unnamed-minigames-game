using Axoloop.Global;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsOnCrown : SingletonMB<StarsOnCrown>
{
    [SerializeField] private StarSlot[] crownStars; // 3 boutons pour recevoir les étoiles
    [SerializeField] MTSDragAndDropBehavior crowndrag; // drag and drop du joueur

    public bool IsEmpty {get; private set;}
    public bool IsFull { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        OnStarsChanged();
    }

    public void OnStarsChanged()
    {
        IsEmpty = true;
        IsFull = true;
        foreach (StarSlot starSlot in crownStars)
        {
            IsEmpty &= starSlot.IsEmpty;
            IsFull &= !starSlot.IsEmpty;
        }

        crowndrag.enabled = IsFull;
    }

    public void PlaceStar(Sprite star)
    {
        for (int i = 0; i < crownStars.Length; i++)
        {
            if (crownStars[i].IsEmpty)
            {
                crownStars[i].SetStar(star);
                OnStarsChanged();
                break;
            }
        }
    }

    public Sprite[] GetPlayerCrownSprites()
    {
        Sprite[] sprites = new Sprite[crownStars.Length];

        for (int i = 0; i < crownStars.Length; i++)
        {
            sprites[i] = crownStars[i].GetStar();
        }

        return sprites;
    }


}