using AxoLoop.Minigames.MatchTheStars;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;

public static class StarsColorsGenerator
{

    public async static Task SetStarsColor()
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

            Sprite colored = await CreateColoredSpriteAsync(
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

    [BurstCompile]
    public static async Task<Sprite> CreateColoredSpriteAsync(Sprite originalSprite, Color color, string name)
    {
        Texture2D originalTexture = originalSprite.texture;
        Color32[] originalPixels = originalTexture.GetPixels32();

        // Traitement lourd en tâche parallèle
        Color32[] newPixels = await Task.Run(() =>
        {
            Color32[] tempPixels = new Color32[originalPixels.Length];

            for (int i = 0; i < originalPixels.Length; i++)
            {
                Color32 pixel = originalPixels[i];

                if (pixel.a > 25)
                {
                    float alpha = pixel.a / 255f;
                    Color blended = color * new Color(pixel.r / 255f, pixel.g / 255f, pixel.b / 255f, alpha);
                    blended.a = 1f;

                    tempPixels[i] = blended;
                }
                else
                {
                    tempPixels[i] = pixel;
                }
            }
            return tempPixels;
        });

        await Task.Delay(500);

        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.ARGB32, false);
        newTexture.filterMode = FilterMode.Point;
        newTexture.SetPixels32(newPixels);
        newTexture.Apply();

        Sprite newSprite = Sprite.Create(newTexture, originalSprite.rect, new Vector2(0.5f, 0.5f), originalSprite.pixelsPerUnit);
        newSprite.name = name;

        return newSprite;
    }

}
