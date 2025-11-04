using UnityEngine;

namespace PG.Localization
{
    public class LocalizedGameObject : MonoBehaviour
    {
        [SerializeField] private ObjectLanguageElement[] _languageElements;

        [System.Serializable]
        private class ObjectLanguageElement
        {
            public string language;
            public GameObject gameObject;
        }
        private void Start()
        {
            LocalizeGameObject();
        }
        public void LocalizeGameObject()
        {
            for (int i = 0; i < _languageElements.Length; i++)
            {
                _languageElements[i].gameObject.SetActive(_languageElements[i].language == LocalizationSystem.instance.currentLanguage);
            }
        }
    }
}
