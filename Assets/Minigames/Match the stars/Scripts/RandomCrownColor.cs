using System.Collections.Generic;
using UnityEngine;

public class RandomCrownColor : MonoBehaviour
{
    [SerializeField] private Sprite[] starsSpritesList;
    [SerializeField] private Sprite[] coloredStarsSpritesList;

    void Awake()
    {
        SetStarsColor();
    }

    public void SetStarsColor()
    {
        int tripletCount = starsSpritesList.Length / 3;

        if (starsSpritesList.Length % 3 != 0)
        {
            Debug.LogError("Le nombre de sprites doit être divisible par 3 pour faire des triplets !");
            return;
        }

        coloredStarsSpritesList = new Sprite[starsSpritesList.Length];

        List<Color> colors = new List<Color>();
        List<string> colorNames = new List<string>();

        for (int i = 0; i < tripletCount; i++)
        {
            Color c = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
            colors.Add(c);
            colorNames.Add(ColorUtility.ToHtmlStringRGB(c));
        }

        List<int> indices = new List<int>();
        for (int i = 0; i < starsSpritesList.Length; i++)
            indices.Add(i);

        for (int i = 0; i < tripletCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int randomIndex = Random.Range(0, indices.Count);
                int starIndex = indices[randomIndex];
                indices.RemoveAt(randomIndex);

                Sprite colored = CreateColoredSprite(
                    starsSpritesList[starIndex],
                    colors[i],
                    $"stars_{i}_{colorNames[i]}"
                );

                coloredStarsSpritesList[starIndex] = colored;
            }
        }
    }

    private Sprite CreateColoredSprite(Sprite originalSprite, Color color, string name)
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
    
    public Sprite[] GetColoredStarsSpritesList()
    {
        return coloredStarsSpritesList;
    }
}
