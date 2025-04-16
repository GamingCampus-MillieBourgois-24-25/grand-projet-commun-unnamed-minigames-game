using UnityEngine;

public class AddCombo : MonoBehaviour
{
    private int nbMiniGamesWin;
    void Awake()
    {
        WinAddCombo();
    }

    private void WinAddCombo()
    {
        nbMiniGamesWin++;
        if (nbMiniGamesWin == 3)
        {
            ComboManager.Instance.AddCombo(1);
            nbMiniGamesWin = 0;
        }
    }
}
