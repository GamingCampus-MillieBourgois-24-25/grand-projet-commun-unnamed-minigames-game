using System;
using UnityEngine;

public class ResetCurrentScore : MonoBehaviour
{
    private void Start()
    {
        ScoreManager.Instance.ResetCurrentScore();
    }
}
