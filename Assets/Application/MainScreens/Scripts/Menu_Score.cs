using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu_Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textScore;
    public LevelWheel _LevelWheel;
    void Start()
    {
        _textScore.text = _LevelWheel.Score.ToString("n0");
    }
}
