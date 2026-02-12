using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PG.Localization
{
    public class LocalizationChanger : MonoBehaviour
    {
        [SerializeField] private string[] _languages;
        public Dictionary<string, int> languageIDs = new Dictionary<string, int>();
        private void Awake()
        {
            for (int i = 0; i < _languages.Length; i++)
            {
                languageIDs.Add(_languages[i], i);
            }
        }
        public void Change(int value)
        {

            LocalizationSystem.instance.currentLanguage = _languages[value];
            LocalizationSystem.instance.LoadLocalization();
        }
    }
}
