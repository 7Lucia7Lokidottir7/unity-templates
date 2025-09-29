using UnityEngine;

namespace PG.LocalizationSystem
{
    public class LocalizationChanger : MonoBehaviour
    {
        [SerializeField] private string[] _languages;
        public void Change(int value)
        {

            LocalizationSystem.instance.currentLanguage = _languages[value];
            LocalizationSystem.instance.LoadLocalization();
        }
    }
}
