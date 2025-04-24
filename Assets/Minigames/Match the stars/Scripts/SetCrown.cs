using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCrown : MonoBehaviour
{
    [SerializeField] private RandomCrownColor randomCrownSource; // script qui contient coloredStarsSpritesList
    [SerializeField] private Image[] starImages; // les 3 images à assigner

    void Start()
    {
        SetThreeStars();
    }

    public void SetThreeStars()
    {
        if (randomCrownSource == null || starImages.Length < 3)
        {
            Debug.LogWarning("Références manquantes !");
            return;
        }

        Sprite[] allColoredStars = randomCrownSource.GetColoredStarsSpritesList();

        if (allColoredStars.Length < 3)
        {
            Debug.LogWarning("Pas assez de sprites dans la liste !");
            return;
        }

        List<int> usedIndices = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int randIndex;
            do
            {
                randIndex = Random.Range(0, allColoredStars.Length);
            } while (usedIndices.Contains(randIndex));

            usedIndices.Add(randIndex);
            starImages[i].sprite = allColoredStars[randIndex];
        }
    }
    
    public Sprite[] GetCrownSprites()
    {
        Sprite[] result = new Sprite[starImages.Length]; // Utilisation de starImages à la place de crownButtons

        for (int i = 0; i < starImages.Length; i++)
        {
            Image img = starImages[i];
            result[i] = img != null ? img.sprite : null; // Récupère le sprite ou null si l'image est vide
        }

        return result;
    }

}