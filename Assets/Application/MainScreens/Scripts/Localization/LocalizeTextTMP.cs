using TMPro;
using UnityEngine;

namespace Assets.SimpleLocalization.Scripts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizeTextTMP : MonoBehaviour
    {
        [SerializeField] private string _localizationKey;
        public string LocalizationKey
        {
            get => _localizationKey;
            set
            {
                if (value != _localizationKey)
                {
                    _localizationKey = value;
                    Localize();
                }
            }
        }

        public void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
            GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}