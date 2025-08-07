using System.Collections;
using TMPro;
using UnityEngine;

namespace PG.HungerSystem
{
    public class HungerText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textObject;
        [SerializeField] private Hunger _hunger;
        [SerializeField] private bool _showMaxValue;

        private void OnEnable()
        {
            _hunger.onValueChanged += OnValueChange;
        }

        private void OnDisable()
        {
            _hunger.onValueChanged -= OnValueChange;
        }
        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(0.04f);
            _textObject.text = _showMaxValue ? $"{_hunger.value}/{_hunger.maxValue}" : _hunger.value.ToString();
        }
        void OnValueChange(float value)
        {
            _textObject.text = _showMaxValue ? $"{value}/{_hunger.maxValue}" : value.ToString();
        }
    }
}
