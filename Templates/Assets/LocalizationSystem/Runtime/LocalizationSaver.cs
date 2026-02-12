using UnityEngine;

namespace PG.Localization
{
    public class LocalizationSaver : MonoBehaviour
    {
        [SerializeField] private string _localizationParameter = "Language";
        private void OnEnable()
        {
            LocalizationSystem.instance.localizationChanged += OnLocalizationChanged;
        }
        private void OnDisable()
        {
            LocalizationSystem.instance.localizationChanged -= OnLocalizationChanged;
        }
        void OnLocalizationChanged(string language)
        {
            PlayerPrefs.SetString(_localizationParameter, language);
        }
    }
}
