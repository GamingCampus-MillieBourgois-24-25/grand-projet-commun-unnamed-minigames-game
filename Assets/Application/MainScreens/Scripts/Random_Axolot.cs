using Assets._Common.Scripts;
using UnityEngine;
using UnityEngine.UI;
using static Assets._Common.Scripts.BeatSyncListener;

public class Random_Axolot : MonoBehaviour
{
    [SerializeField] private GameObject _axolotSprite;
    [SerializeField] private Sprite[] _axolotSpritesList;
    [SerializeField] bool changeBeatSync = true; 

    private int _currentIndex = -1;

    void Start()
    {
        FirstChange();
    }


    void FirstChange()
    {
        DoChange(6);
    }

    public void ChangeAxolot()
    {
        DoChange(9);
    }

    void DoChange(int lastIndex)
    {
        int newIndex;

        do
        {
            newIndex = Random.Range(0, lastIndex);
        }
        while (newIndex == _currentIndex);

        _currentIndex = newIndex;
        _axolotSprite.GetComponent<Image>().sprite = _axolotSpritesList[newIndex];

        if(changeBeatSync)
            _axolotSprite.GetComponent<BeatSyncListener>()?.SetBeatBehaviour((BeatSyncListener.BeatBehaviour)Random.Range(0, System.Enum.GetValues(typeof(BeatSyncListener.BeatBehaviour)).Length));
    }
}