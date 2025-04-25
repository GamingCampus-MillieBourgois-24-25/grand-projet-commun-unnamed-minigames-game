using UnityEngine;
using UnityEngine.UI;

public class Verif : MonoBehaviour
{
    [SerializeField] private SetCrown _setCrown;
    [SerializeField] private SetStarsToCrown _setStarsToCrown;
    [SerializeField] private Sprite axoHeadWin, axoHeadLoss;
    [SerializeField] private Image axoToChange;

    public void checkWin()
    {
        var testA = _setCrown.GetCrownSprites();
        var testB = _setStarsToCrown.GetPlayerCrownSprites();
        
        var allMatch = true;
        for (int i = 0; i < testA.Length; i++)
        {
            if (testA[i].name != testB[i].name) // Comparaison des noms des sprites
            {
                allMatch = false;
                break;
            }
        }

        axoToChange.sprite = allMatch ? axoHeadWin : axoHeadLoss;
        MiniGameManager.Instance.MiniGameFinished(allMatch);
    }
}