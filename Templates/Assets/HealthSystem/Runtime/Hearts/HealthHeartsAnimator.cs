using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PG.HealthSystem
{
    public class HealthHeartsAnimator : MonoBehaviour, IDamagable, IHealable
    {
        [Header("Hearts / UI")]
        [SerializeField] private List<HealthHeart> _hearts = new List<HealthHeart>();

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

        [SerializeField] private UnityEvent _damageEvent;
        public event Action<float, GameObject> damaged;
        public event Action<float> healed;

        [SerializeField] private int _maxValue = 4;
        [SerializeField] private int _value = 4;

        private Coroutine _animRoutine;
        private Coroutine _damageCooldownCoroutine;

        private void OnDestroy()
        {
            if (_animRoutine != null) StopCoroutine(_animRoutine);
            if (_damageCooldownCoroutine != null) StopCoroutine(_damageCooldownCoroutine);
        }

        private void Awake()
        {
            // Если в инспекторе задано value 0, заполняем до максимума
            if (_value <= 0) _value = _maxValue;
            Setup(_maxValue, _value);
        }

        /// <summary>
        /// Initializes the system. If value <= 0, fills health to max.
        /// Automatically creates or adjusts hearts according to maxValue.
        /// </summary>
        public void Setup(int maxValue, int value = 0)
        {
            _maxValue = maxValue;

            // Реализация логики из комментария: если 0 или меньше, лечим полностью
            if (value <= 0) _value = maxValue;
            else _value = Mathf.Clamp(value, 0, maxValue);

            SetMaxUI();
        }

        public void OnDamage(float damage, bool ignoreDamage = false, GameObject damageObject = null)
        {
            if (!ignoreDamage && !useDamage) return;
            if (_value <= 0) return; // Уже мертвы

            if (_cooldownEnable)
            {
                if (_damageCooldownCoroutine != null) return;
                _damageCooldownCoroutine = StartCoroutine(DamageCooldown());
            }

            int damageInt = (int)damage;
            // Рассчитываем целевое здоровье, не опускаясь ниже нуля
            int targetValue = Mathf.Max(0, _value - damageInt);

            // ВИЗУАЛ: "Ломаем" сердца от текущего последнего до целевого
            // Исправлено: цикл теперь не уходит в минус
            for (int i = _value - 1; i >= targetValue; i--)
            {
                if (i < _hearts.Count && i >= 0)
                {
                    _hearts[i].Damage();
                }
            }

            _value = targetValue;
            _damageEvent?.Invoke();
            damaged?.Invoke(damage, damageObject);

            if (_value == 0 && _deathObject != null && _deathObject.TryGetComponent(out IDeath death))
                death.OnDeath();
        }

        public void OnHeal(float value)
        {
            if (_value >= _maxValue) return;

            int healInt = (int)value;
            int targetValue = Mathf.Clamp(_value + healInt, 0, _maxValue);

            // ВИЗУАЛ: "Чиним" сердца от текущего пустого до целевого
            for (int i = _value; i < targetValue; i++)
            {
                if (i < _hearts.Count)
                {
                    _hearts[i].Heal();
                }
            }

            _value = targetValue;
            healed?.Invoke(value);
        }

        void SetMaxUI()
        {
            // Удаляем лишние, если макс. здоровье уменьшилось
            if (_hearts.Count > _maxValue)
            {
                for (int i = _hearts.Count - 1; i >= _maxValue; i--)
                {
                    if (_hearts[i] != null) Destroy(_hearts[i].gameObject);
                    _hearts.RemoveAt(i);
                }
            }

            // Добавляем недостающие
            if (_hearts.Count < _maxValue)
            {
                for (int i = _hearts.Count; i < _maxValue; i++)
                {
                    // Исправлено: Сердце создается "полным" (Heal) только если его индекс меньше текущего здоровья (_value)
                    // Иначе создаем пустое (разбитое) сердце
                    bool startFull = i < _value;
                    CreateHeart(startFull);
                }
            }
        }

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
        private void CreateHeart(bool heal = false)
        {
            if (_heartPrefab == null || _heartsParent == null) return;

            GameObject heartObj = Instantiate(_heartPrefab, _heartsParent);
            HealthHeart healthHeart = heartObj.GetComponent<HealthHeart>();
            _hearts.Add(healthHeart);

            if (heal)
            {
                healthHeart.Heal();
            }
            else
            {
                // Если сердце должно быть пустым при создании, убедитесь, 
                // что в HealthHeart есть логика для старта в состоянии "Broken" 
                // или вызовите Damage() без звука/эффектов, если это возможно.
                // healthHeart.Damage(); 
            }
        }
    }
}