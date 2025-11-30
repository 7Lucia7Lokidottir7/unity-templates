using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.HealthSystem
{
    public class HealthHeartsAnimator : MonoBehaviour, IDamagable, IHealable
    {
        [Header("Hearts / UI")]
        [SerializeField] private List<HealthHeart> _hearts = new List<HealthHeart>(); // you can seed starting ones from the scene

        [Tooltip("Prefab of a single heart with an Animator component")]
        [SerializeField] private GameObject _heartPrefab;

        [Tooltip("Parent transform where hearts will be instantiated")]
        [SerializeField] private Transform _heartsParent;



        [Header("Damage Cooldown")]
        [SerializeField] private bool _cooldownEnable;
        [SerializeField] private float _damageCooldown = 0.2f;

        [Header("Gameplay")]
        public bool useDamage = true;
        [SerializeField] private GameObject _deathObject;

        public event Action<float> damaged;
        public event Action<float> healed;

        [SerializeField] private int _maxValue = 4;
        private int _value;

        private Coroutine _animRoutine;
        private Coroutine _damageCooldownCoroutine;

        private void OnDestroy()
        {
            if (_animRoutine != null) StopCoroutine(_animRoutine);
            if (_damageCooldownCoroutine != null) StopCoroutine(_damageCooldownCoroutine);
        }

        private void Awake()
        {
            Setup(_maxValue, _value);
        }

        /// <summary>
        /// Initializes the system. If value <= 0, fills health to max.
        /// Automatically creates or adjusts hearts according to maxValue.
        /// </summary>
        public void Setup(int maxValue, int value = 0)
        {
            _maxValue = maxValue;
            _value = value;
            SetMaxUI();
        }

        public void OnDamage(float damage, bool ignoreDamage = false)
        {
            if (!ignoreDamage && !useDamage) return;

            if (_cooldownEnable)
            {
                if (_damageCooldownCoroutine != null) return;
                _damageCooldownCoroutine = StartCoroutine(DamageCooldown());
            }

            for (int i = _value - 1; i >= _value - (int)damage; i--)
            {
                _hearts[i].Damage();
            }

            _value -= (int)damage;


            damaged?.Invoke(damage);

            if (_deathObject != null && _deathObject.TryGetComponent(out IDeath death))
                death.OnDeath();
        }

        public void OnHeal(float value)
        {
            for (int i = _value; i < Mathf.Clamp(_value + (int)value, 0, _maxValue); i++)
            {
                _hearts[i].Heal();
            }

            _value += (int)value;
            healed?.Invoke(value);
        }

        void SetMaxUI()
        {
            if (_hearts.Count < _maxValue)
            {
                for (int i = _hearts.Count; i < _maxValue; i++)
                {
                    CreateHeart(true, true);
                }
            }
            if (_hearts.Count > _maxValue)
            {
                for (int i = _hearts.Count - 1; i >= _maxValue; i--)
                {
                    Destroy(_hearts[i].gameObject);
                    _hearts.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Manually adds one heart (UI only, does not affect _maxValue).
        /// Useful if you want to pre-generate some hearts.
        /// </summary>
        public void AddOneHeart()
        {
            CreateHeart();
        }




        private IEnumerator DamageCooldown()
        {
            useDamage = false;
            yield return new WaitForSeconds(_damageCooldown);
            useDamage = true;
            _damageCooldownCoroutine = null;
        }


        /// <summary>
        /// Instantiates a new heart from the prefab and adds it to the list.
        /// </summary>
        private void CreateHeart(bool heal = false, bool addToList = true)
        {
            HealthHeart healthHeart = Instantiate(_heartPrefab, _heartsParent).GetComponent<HealthHeart>();
            _hearts.Add(healthHeart);

            if (heal)
            {
                healthHeart.Heal();
            }
        }
    }
}
