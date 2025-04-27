using AxoLoop.Minigames.MatchTheStars;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class StarsColorsGenerator
{

    public static void SetStarsColor()
    {
        // récupère les sprites et les couleurs disponibles
        var starsSpritesList = MatchTheStarsMinigameData.AvailableSpritesList;
        var availableColors = (PlayerPrefs.GetString("ColorblindMode", "False") == "True") ? MatchTheStarsMinigameData.AvailableDaltonienColors : MatchTheStarsMinigameData.AvailableColors;
        var coloredStarsSpritesList = new Sprite[MatchTheStarsMinigameData.StarsCount];

        // Configure la difficulté en évitant d'inclure les derniers sprites en cas de facilité
        int maxIndex = MatchTheStarsMinigameData.StarsCount;
        switch (MatchTheStarsMinigameData.Difficulty)
        {
            case MTSDifficulty.Easy:
                maxIndex = 3;
                break;
            case MTSDifficulty.Medium:
                maxIndex = 5;
                break;
            case MTSDifficulty.Hard:
                maxIndex = 9;
                break;
        }

        // Pioche un sprite et une couleur par emplacement d'étoile
        for (int i = 0; i < coloredStarsSpritesList.Length; i++)
        {
            int randomColorIndex = Random.Range(0, availableColors.Length);
            int randomSpriteIndex = Random.Range(0, maxIndex);
            Color color = availableColors[randomColorIndex];
            string colorName = ColorUtility.ToHtmlStringRGB(color);

            Sprite colored = CreateColoredSprite(
                starsSpritesList[i],
                color,
                $"stars_{i}_{colorName}"
            );

            coloredStarsSpritesList[i] = colored;
        }

        MatchTheStarsMinigameData.ColoredStarsSpritesList = coloredStarsSpritesList;
    }


    public static void SetThreeStars()
    {
        // Récupère les images de la couronne
        Image[] starImages = MatchTheStarsMinigameData.CrownStarsImages;
        Sprite[] allColoredStars = MatchTheStarsMinigameData.ColoredStarsSpritesList;

        if (starImages.Length < 3)
        {
            Debug.LogWarning("Couronne : Références manquantes !");
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

        MatchTheStarsMinigameData.CrownStarsImages = starImages;
    }


    private static Sprite CreateColoredSprite(Sprite originalSprite, Color color, string name)
    {
        Texture2D originalTexture = originalSprite.texture;
        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height);
        newTexture.filterMode = FilterMode.Point;

        for (int x = 0; x < originalTexture.width; x++)
        {
            for (int y = 0; y < originalTexture.height; y++)
            {
                Color pixel = originalTexture.GetPixel(x, y);
                if (pixel.a > 0.1f)
                {
                    pixel = color * pixel;
                    pixel.a = 1f;
                    newTexture.SetPixel(x, y, pixel);
                }
                else
                {
                    newTexture.SetPixel(x, y, pixel);
                }
            }
        }

        newTexture.Apply();

        Sprite newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f), originalSprite.pixelsPerUnit);
        newSprite.name = name; // Donne un nom utile

        return newSprite;
    }
}
