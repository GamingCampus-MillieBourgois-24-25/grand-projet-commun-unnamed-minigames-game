using DG.Tweening;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBag : MonoBehaviour
{
    [SerializeField] private GameObject closed;
    [SerializeField] private GameObject opened;
    [SerializeField] private StarsInBag _starsInBag;


    Vector3 closedPosition;
    Vector3 openedPosition;
    public void Start()
    {
        closedPosition = closed.transform.position;
        openedPosition = opened.transform.position;
    }

    public void OpenTheBag()
    {
        if (closed != null)
        {
            closed.SetActive(false);
            opened.SetActive(true);
            _starsInBag.SetNineStarsInOrder();
            PlayOpenAnimation();
        }
    }



    public void CloseTheBag()
    {
        if (StarsOnCrown.Instance.IsEmpty)
        {
            ForceCloseTheBag();
        }
        else 
        {
            PlayShakeAnimation();
        }
    }

    public void ForceCloseTheBag()
    {
        closed.SetActive(true);
        opened.SetActive(false);
        _starsInBag.SetNineStarsInOrder();
        PlayCloseAnimation();
    }

    // create a dotween animation that add a little bounce to the opened gameobject
    public void PlayOpenAnimation()
    {

        // Create a bounce animation: quick uplift and slow return
        opened.transform.DOLocalMoveY(opened.transform.localPosition.y + 30f, 0.15f) // Uplift by 10 units in 0.2 seconds
            .SetEase(Ease.InQuad) // Fast uplift
            .OnComplete(() =>
            {
                opened.transform.DOLocalMoveY(opened.transform.localPosition.y - 30f, 0.2f) // Return to original position in 0.4 seconds
                    .SetEase(Ease.OutQuad); // Slow return
            });
        opened.transform.DOScaleY(1.05f, 0.15f) // Uplift by 10 units in 0.2 seconds
            .SetEase(Ease.InQuad) // Fast uplift
            .OnComplete(() =>
            {
                opened.transform.DOScaleY(1f, 0.2f) // Return to original position in 0.4 seconds
                    .SetEase(Ease.OutQuad); // Slow return
            });
        
    }

    public void PlayCloseAnimation()
    {

        // Create a bounce animation: quick uplift and slow return using offset
        closed.transform.DOLocalMoveY(closed.transform.localPosition.y - 30f, 0.15f)
            .SetEase(Ease.InQuad) // Fast uplift
            .OnComplete(() =>
            {
                closed.transform.DOLocalMoveY(closed.transform.localPosition.y + 30f, 0.2f)
                    .SetEase(Ease.OutQuad); // Slow return
            });


        closed.transform.DOScaleY(0.95f, 0.15f) 
            .SetEase(Ease.InQuad) // Fast uplift
            .OnComplete(() =>
            {
                closed.transform.DOScaleY(1f, 0.2f) 
                    .SetEase(Ease.OutQuad); // Slow return
            });
        
    }

    public void PlayShakeAnimation()
    {

        opened.transform.DOShakePosition(0.5f, new Vector3(10f, 10f, 0), 10, 90, false, true)
            .SetEase(Ease.InQuad)
            .OnComplete(() => opened.transform.DOMove(openedPosition, 0.1f));

    }
}