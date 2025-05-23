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
        // r�cup�re les sprites et les couleurs disponibles
        var starsSpritesList = MatchTheStarsMinigameData.AvailableSpritesList;
        var availableColors = (PlayerPrefs.GetString("ColorblindMode", "False") == "True") ? MatchTheStarsMinigameData.AvailableDaltonienColors : MatchTheStarsMinigameData.AvailableColors;
        var coloredStarsSpritesList = new Sprite[MatchTheStarsMinigameData.StarsCount];

        // Configure la difficult� en �vitant d'inclure les derniers sprites en cas de facilit�
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

        // Pioche un sprite et une couleur par emplacement d'�toile
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
        // R�cup�re les images de la couronne
        Image[] starImages = MatchTheStarsMinigameData.CrownStarsImages;
        Sprite[] allColoredStars = MatchTheStarsMinigameData.ColoredStarsSpritesList;

        if (starImages.Length < 3)
        {
            Debug.LogWarning("Couronne : R�f�rences manquantes !");
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

    public static async Task<Sprite> CreateColoredSpriteAsync(Sprite originalSprite, Color color, string name)
    {
        // Mise en cache des �l�ments
        Texture2D originalTexture = originalSprite.texture;
        Color32[] originalPixels = originalTexture.GetPixels32();
        Rect rect = originalSprite.rect;
        float pixelsPerUnit = originalSprite.pixelsPerUnit;
        int width = originalTexture.width;
        int height = originalTexture.height;

        byte colorR = (byte)(color.r * 255f);
        byte colorG = (byte)(color.g * 255f);
        byte colorB = (byte)(color.b * 255f);

        // Traitement parall�le, sans toucher � UnityEngine
        Color32[] newPixels = await Task.Run(() =>
        {
            Color32[] tempPixels = new Color32[originalPixels.Length];

            for (int i = 0; i < originalPixels.Length; i++)
            {
                Color32 pixel = originalPixels[i];

                if (pixel.a > 25)
                {
                    byte r = (byte)((colorR * pixel.r) / 255);
                    byte g = (byte)((colorG * pixel.g) / 255);
                    byte b = (byte)((colorB * pixel.b) / 255);

                    tempPixels[i] = new Color32(r, g, b, 255);
                }
                else
                {
                    tempPixels[i] = pixel;
                }
            }

            return tempPixels;
        });

        // Retour au thread principal, application des calculs effectu�s en parall�le
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        newTexture.filterMode = FilterMode.Point;
        newTexture.SetPixels32(newPixels);
        newTexture.Apply();

        Sprite newSprite = Sprite.Create(newTexture, rect, new Vector2(0.5f, 0.5f), pixelsPerUnit);
        newSprite.name = name;

        return newSprite;
    }


}
