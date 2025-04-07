using System.Collections;
using UnityEngine;


namespace Axoloop.Global.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIObjectGroup : MonoBehaviour
    {

        [SerializeField] bool _startActive = true;
        CanvasGroup _canvasGroupRef;
        UIObjectGroupData _uiObjectGroupData;

        private bool _isEnabled = true;

        private void Awake()
        {
            _canvasGroupRef = GetComponent<CanvasGroup>();
            _uiObjectGroupData = new UIObjectGroupData
            {
                alpha = _canvasGroupRef.alpha,
                interactable = _canvasGroupRef.interactable,
                blocksRaycasts = _canvasGroupRef.blocksRaycasts
            };
        }

        void Start()
        {
            if (!_startActive)
            {
                SetDisabled();
            }
        }

        void SetDisabled()
        {
            _canvasGroupRef.alpha = 0;
            _canvasGroupRef.interactable = false;
            _canvasGroupRef.blocksRaycasts = false;
        }
        void SetEnabled()
        {
            _canvasGroupRef.alpha = _uiObjectGroupData.alpha;
            _canvasGroupRef.interactable = _uiObjectGroupData.interactable;
            _canvasGroupRef.blocksRaycasts = _uiObjectGroupData.blocksRaycasts;
        }


        /// <summary>
        /// Ouverture de l'objet en fondu
        /// </summary>
        public void EnableComponent()
        {
            if (_isEnabled) return;
            _isEnabled = true;
            StartCoroutine(EnableComponentCoroutine());
        }

        /// <summary>
        /// fermeture de l'objet en fondu
        /// </summary>
        public void DisableComponent()
        {
            if (!_isEnabled) return;
            _isEnabled = false;
            StartCoroutine(DisableComponentCoroutine());
        }

        IEnumerator EnableComponentCoroutine()
        {
            float duration = GameSettings.UIObjectFadeDuration;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0, _uiObjectGroupData.alpha, elapsedTime / duration);
                _canvasGroupRef.alpha = alpha;
                yield return null;
            }
            SetEnabled();
        }

        IEnumerator DisableComponentCoroutine()
        {
            _canvasGroupRef.interactable = false;
            float duration = GameSettings.UIObjectFadeDuration;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(_uiObjectGroupData.alpha, 0, elapsedTime / duration);
                _canvasGroupRef.alpha = alpha;
                yield return null;
            }
            SetDisabled();
        }


        private struct UIObjectGroupData
        {
            public float alpha;
            public bool interactable;
            public bool blocksRaycasts;
        }
    }
}

