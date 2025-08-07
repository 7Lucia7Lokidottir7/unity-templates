using System.Collections;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.UI;

namespace PG.HungerSystem
{
    public class HungerSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Hunger _hunger;

        private void OnEnable()
        {
            _hunger.onValueChanged += OnValueChange;
            _slider.value = _hunger.value;
        }

        private void OnDisable()
        {
            _hunger.onValueChanged -= OnValueChange;
        }
        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(0.04f);
            _slider.value =_hunger.value;
        }
        void OnValueChange(float value)
        {
            _slider.value = value;
        }
    }
}
