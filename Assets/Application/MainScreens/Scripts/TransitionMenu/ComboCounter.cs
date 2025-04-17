using UnityEngine;
using TMPro;
public class ComboCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;
    void Start()
    {
        int _combo = ComboManager.Instance.GetCombo();
        
        if (_combo <= 1)
        {
            comboText.enabled = false;
        }
        else
        {
            comboText.text = "x" + _combo.ToString("n0");
        }
    }
}
