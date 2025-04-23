using UnityEngine;

public class Verif : MonoBehaviour
{
    [SerializeField] private SetCrown _setCrown;
    [SerializeField] private SetStarsToCrown _setStarsToCrown;

    public void checkWin()
    {
        var testA = _setCrown.GetCrownSprites();
        var testB = _setStarsToCrown.GetPlayerCrownSprites();
        
        bool allMatch = true;
        for (int i = 0; i < testA.Length; i++)
        {
            if (testA[i].name != testB[i].name) // Comparaison des noms des sprites
            {
                allMatch = false;
                break;
            }
        }

        if (allMatch)
        {
            Debug.Log("WINNNN");
        }
        else
        {
            Debug.Log("Lose");
        }
    }
}