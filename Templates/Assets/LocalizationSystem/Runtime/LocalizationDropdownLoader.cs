using TMPro;
using UnityEngine;

namespace PG.Localization
{
    public class LocalizationDropdownLoader : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _localizationDropdown;
        [SerializeField] private LocalizationChanger _localizationChanger;
        [SerializeField] private string _localizationParameter = "Language";
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (PlayerPrefs.HasKey(_localizationParameter))
            {
                _localizationDropdown.value = _localizationChanger.languageIDs[PlayerPrefs.GetString(_localizationParameter)];
            }
        }
    }
}
