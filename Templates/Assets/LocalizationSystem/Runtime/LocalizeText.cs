using TMPro;
using UnityEngine;

namespace PG.LocalizationSystem
{
    public class LocalizeText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textObject;
        [SerializeField] private string _key;
        public string key => _key;

        [ContextMenu("Get Cache")]
        private void GetCache() => TryGetComponent(out _textObject);
        [ContextMenu("Get Key Cache")]
        private void GetKeyCache() => _key = _textObject.text;
        private void Start()
        {
            Localize();
        }
        public void Localize()
        {
            _textObject.text = LocalizationSystem.instance.GetLocalizedValue(_key, _textObject.text);
        }
    }
}
