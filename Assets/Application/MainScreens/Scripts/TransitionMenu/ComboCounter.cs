using UnityEngine;
using TMPro;
public class ComboCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;
    void Start()
    {
        int _combo = ComboManager.Instance.GetCombo();
        
        if (_combo <= 2)
        {
            comboText.enabled = false;
        }
        else
        {
            comboText.text = _combo.ToString("n0");
        }
    }
}
