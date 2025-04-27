using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace Assets.SimpleLocalization.Scripts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizeTextTMP : MonoBehaviour
    {
        [SerializeField] private string _localizationKey;
        private TextMeshProUGUI tmp;
        
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
            tmp = GetComponent<TextMeshProUGUI>();
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
            if (string.IsNullOrEmpty(_localizationKey)) return;
            tmp.text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}