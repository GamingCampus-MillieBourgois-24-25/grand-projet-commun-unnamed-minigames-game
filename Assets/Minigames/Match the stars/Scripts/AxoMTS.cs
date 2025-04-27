using Axoloop.Global;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.Match_the_stars.Scripts
{
    public class AxoMTS : SingletonMB<AxoMTS>
    {
        [SerializeField] private Image[] axoHeadWin, axoHeadLoss;
        [SerializeField] Image axoImage;

        // create a dotween animation that add a little bounce to the opened gameobject
        public void PlayOpenAnimation()
        {
            transform.DOScaleY(1.05f, 0.15f) // Uplift by 10 units in 0.2 seconds
                .SetEase(Ease.InQuad) // Fast uplift
                .OnComplete(() =>
                {
                    transform.DOScaleY(1f, 0.2f) // Return to original position in 0.4 seconds
                        .SetEase(Ease.OutQuad); // Slow return
                });
        }

        public void SetHappy()
        {
            axoImage.enabled = false;
            //enable one random win image
            int randomIndex = Random.Range(0, axoHeadWin.Length);
            axoHeadWin[randomIndex].gameObject.SetActive(true);
        }

        public void SetAngry()
        {
            axoImage.enabled = false;
            int randomIndex = Random.Range(0, axoHeadLoss.Length);
            axoHeadLoss[randomIndex].gameObject.SetActive(true);
        }
    }
}
