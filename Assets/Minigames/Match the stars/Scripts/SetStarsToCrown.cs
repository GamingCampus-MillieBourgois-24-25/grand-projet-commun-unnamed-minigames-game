using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetStarsToCrown : MonoBehaviour
{
    [SerializeField] private Button[] crownButtons; // 3 boutons pour recevoir les étoiles

    public void OnStarButtonClicked(Button clickedButton)
    {
        Image clickedImage = clickedButton.GetComponent<Image>();
        if (clickedImage == null || clickedImage.sprite == null)
        {
            Debug.Log("tetetetete");
            return;
        }
        foreach (Button crownButton in crownButtons)
        {
            Image crownImage = crownButton.GetComponent<Image>();
            if (crownImage != null && crownImage.sprite == null)
            {
                crownImage.sprite = clickedImage.sprite;
                return; // On arrête après avoir assigné
            }
        }
    }

    public Sprite[] GetPlayerCrownSprites()
    {
        Sprite[] sprites = new Sprite[crownButtons.Length];

        for (int i = 0; i < crownButtons.Length; i++)
        {
            Image img = crownButtons[i].GetComponent<Image>();
            sprites[i] = img != null ? img.sprite : null;
        }

        return sprites;
    }


}