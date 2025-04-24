using UnityEngine;
using UnityEngine.UI;

public class StarsInBag : MonoBehaviour
{
    [SerializeField] private RandomCrownColor randomCrownSource; // script qui contient coloredStarsSpritesList
    [SerializeField] private Button[] starButtons; // les 9 boutons à assigner

    public void SetNineStarsInOrder()
    {
        if (randomCrownSource == null || starButtons.Length < 9)
        {
            Debug.LogWarning("Références manquantes ou pas assez de boutons !");
            return;
        }

        Sprite[] allColoredStars = randomCrownSource.GetColoredStarsSpritesList();

        if (allColoredStars.Length < 9)
        {
            Debug.LogWarning("Pas assez de sprites dans la liste !");
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            Image buttonImage = starButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = allColoredStars[i];
            }
        }
    }
}