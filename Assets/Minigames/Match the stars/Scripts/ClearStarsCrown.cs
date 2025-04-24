using UnityEngine.UI;
using UnityEngine;

public class ClearStarsCrown : MonoBehaviour
{
    public void ClearButton(Button clickedButton)
    {
        clickedButton.GetComponent<Image>().sprite = null;
    }
}
