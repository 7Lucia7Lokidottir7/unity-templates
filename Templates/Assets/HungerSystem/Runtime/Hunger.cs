using UnityEngine;

namespace PG.HungerSystem
{
    public class Hunger : MonoBehaviour, IEating, IHungry
    {
        public float maxValue = 100;
        public float minValue = 0;
        public float value
        {
            get => _value;
            set
            {
                _value = value;
                _value = Mathf.Clamp(value, minValue, maxValue);
                onValueChanged?.Invoke(_value);
                if (_value == minValue)
                {
                    lowValueAction?.Invoke();
                }
            }
        }
        private float _value;

        public event System.Action<float> onValueChanged;
        public event System.Action lowValueAction;

        [SerializeField] private float _delayDecrease;
        public event System.Action<float> eated;
        public event System.Action<float> hungred;

        private float _baseTime;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            value = maxValue;
            _baseTime = Time.fixedTime + _delayDecrease;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Time.fixedTime > _baseTime)
            {
                _baseTime = Time.fixedTime;
                Hungry(1);
            }
        }

        public void Eat(float eatValue)
        {
            value += eatValue;
            eated?.Invoke(eatValue);
        }

        public void Hungry(float hungryValue)
        {
            value -= hungryValue;
            hungred?.Invoke(hungryValue);
        }
    }
}
