using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Random_Axolot : MonoBehaviour
{
    [SerializeField] private GameObject _axolotSprite;
    [SerializeField] private Sprite[] _axolotSpritesList;
    
    void Awake()
    {
        ChangeAxolot();
    }

    public void ChangeAxolot()
    {
        _axolotSprite.GetComponent<Image>().sprite = _axolotSpritesList[Random.Range(1, 4)];
    }
}
