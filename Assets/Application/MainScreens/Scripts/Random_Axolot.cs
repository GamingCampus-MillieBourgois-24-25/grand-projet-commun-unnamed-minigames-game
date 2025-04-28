using Assets._Common.Scripts;
using UnityEngine;
using UnityEngine.UI;
using static Assets._Common.Scripts.BeatSyncListener;

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


        _axolotSprite.GetComponent<BeatSyncListener>()?.SetBeatBehaviour((BeatSyncListener.BeatBehaviour)Random.Range(0, System.Enum.GetValues(typeof(BeatSyncListener.BeatBehaviour)).Length));
    }
}