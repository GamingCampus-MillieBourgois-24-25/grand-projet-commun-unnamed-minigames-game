using AxoLoop.Minigames.MatchTheStars;
using AxoLoop.Minigames.MatchTheStars;
using UnityEngine;
using UnityEngine.UI;

public class StarSlot : MonoBehaviour
{
    [SerializeField] bool isCrown = false;
    public bool IsEmpty { get; private set; } = true;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        SetEmpty();
    }

    void SetEmpty()
    {
        image.sprite = MatchTheStarsMinigameData.EmptyStarSprite;
        IsEmpty = true;
    }

    public void OnClick()
    {
        if (IsEmpty)
            return;
        Sprite starSprite = image.sprite;
        SetEmpty();

        if (isCrown)
        {
            StarsInBag.Instance.RaplaceTakenStar(starSprite);
            StarsOnCrown.Instance.OnStarsChanged();
        }
        else
        {
            StarsOnCrown.Instance.PlaceStar(starSprite);
        }
    }

    public void SetStar(Sprite starSprite)
    {
        image.sprite = starSprite;
        IsEmpty = false;
    }

    public Sprite GetStar()
    {
        return image.sprite;
    }
}