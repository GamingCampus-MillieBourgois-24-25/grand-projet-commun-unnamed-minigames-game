using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBag : MonoBehaviour
{
    [SerializeField] private GameObject bagToClose;
    [SerializeField] private GameObject bagToOpen;
    [SerializeField] private StarsInBag _starsInBag;
    
    public void CloseBag()
    {
        if (bagToClose != null)
        {
            bagToClose.SetActive(false);
            bagToOpen.SetActive(true);
            _starsInBag.SetNineStarsInOrder();
        }
    }
}