using UnityEngine;
using UnityEngine.UI;

public class Random_Axolot : MonoBehaviour
{
    [SerializeField] private GameObject _axolotSprite;
    [SerializeField] private Sprite[] _axolotSpritesList;

    private int _lastIndex = -1;

    void Start()
    {
        ChangeAxolot();
    }

    public void ChangeAxolot()
    {
        int newIndex;

        do
        {
            newIndex = Random.Range(0, 9);
        }
        while (newIndex == _lastIndex);

        _lastIndex = newIndex;
        _axolotSprite.GetComponent<Image>().sprite = _axolotSpritesList[newIndex];
    }
}