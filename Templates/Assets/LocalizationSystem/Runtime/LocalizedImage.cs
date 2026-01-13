using UnityEngine;
using UnityEngine.UI;

namespace PG.Localization
{
    public class LocalizedImage : MonoBehaviour
    {
        [SerializeField] private Image _imageObject;
        [SerializeField] private SpriteLanguageElement[] _languageElements;
        [ContextMenu("Get Cache")]
        private void GetCache() => TryGetComponent(out _imageObject);
        private void Start()
        {
            LocalizeImage();
        }
        public void LocalizeImage()
        {
            for (int i = 0; i < _languageElements.Length; i++)
            {
                if (_languageElements[i].language == LocalizationSystem.instance.currentLanguage)
                {
                    _imageObject.sprite = _languageElements[i].sprite;
                    return;
                }
            }
            _imageObject.sprite = _languageElements[0].sprite;
        }
        public Sprite GetLocalizedValue()
        {
            for (int i = 0; i < _languageElements.Length; i++)
            {
                if (_languageElements[i].language == LocalizationSystem.instance.currentLanguage)
                {
                    return _languageElements[i].sprite;
                }
            }
            return _languageElements[0].sprite;
        }
    }
}
