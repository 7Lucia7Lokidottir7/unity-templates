using System;
using System.Collections;
using UnityEngine;

namespace PG.HealthSystem
{
    public class HealthHeartsAnimator : MonoBehaviour, IDamagable, IHealable
    {
        [SerializeField] private Animator[] _hearts;
        [SerializeField] private float _heartCapacity = 1f;
        [SerializeField] private string _fillParam = "fill";
        [SerializeField] private string _hitTrigger = "hit";
        [SerializeField] private string _healTrigger = "heal";
        [SerializeField] private float _unitsPerSecond = 20f;

        [SerializeField] private bool _cooldownEnable;
        [SerializeField] private float _damageCooldown = 0.2f;

        public bool useDamage;
        [SerializeField] private GameObject _deathObject;

        public event Action<float> damaged;
        public event Action<float> healed;

        private float _maxValue;
        private float _value;
        private float _target;

        private Coroutine _animRoutine;
        private Coroutine _damageCooldownCoroutine;

        private void OnDestroy()
        {
            if (_animRoutine != null) StopCoroutine(_animRoutine);
            if (_damageCooldownCoroutine != null) StopCoroutine(_damageCooldownCoroutine);
        }

        public void Setup(float maxValue, float value = 0)
        {
            _maxValue = maxValue;
            _value = value <= 0 ? maxValue : value;
            _target = _value;
            UpdateHearts(_value);
        }

        public void OnDamage(float damage, bool ignoreDamage = false)
        {
            if (!ignoreDamage && !useDamage) return;
            if (_cooldownEnable)
            {
                if (_damageCooldownCoroutine != null) return;
                _damageCooldownCoroutine = StartCoroutine(DamageCooldown());
            }

            _target = Mathf.Clamp(_target - Mathf.Abs(damage), 0f, _maxValue);
            damaged?.Invoke(damage);
            TriggerAll(_hitTrigger);
            EnsureAnimator();
            if (_target <= 0f && _deathObject != null && _deathObject.TryGetComponent(out IDeath death)) death.OnDeath();
        }

        public void OnHeal(float value)
        {
            _target = Mathf.Clamp(_target + Mathf.Abs(value), 0f, _maxValue);
            healed?.Invoke(value);
            TriggerAll(_healTrigger);
            EnsureAnimator();
        }

        void EnsureAnimator()
        {
            if (_animRoutine == null) _animRoutine = StartCoroutine(AnimateToTarget());
        }

        IEnumerator AnimateToTarget()
        {
            while (true)
            {
                if (Mathf.Approximately(_value, _target)) break;
                float step = _unitsPerSecond * Time.unscaledDeltaTime;
                if (_value < _target) _value = Mathf.Min(_value + step, _target);
                else _value = Mathf.Max(_value - step, _target);
                UpdateHearts(_value);
                yield return null;
            }
            _animRoutine = null;
        }

        void UpdateHearts(float value)
        {
            if (_hearts == null || _hearts.Length == 0 || _heartCapacity <= 0f) return;
            for (int i = 0; i < _hearts.Length; i++)
            {
                float v = value - i * _heartCapacity;
                float fill = Mathf.Clamp01(v / _heartCapacity);
                if (_hearts[i] != null) _hearts[i].SetFloat(_fillParam, fill);
            }
        }

        void TriggerAll(string trigger)
        {
            if (string.IsNullOrEmpty(trigger) || _hearts == null) return;
            for (int i = 0; i < _hearts.Length; i++)
            {
                if (_hearts[i] == null) continue;
                _hearts[i].ResetTrigger(trigger);
                _hearts[i].SetTrigger(trigger);
            }
        }

        IEnumerator DamageCooldown()
        {
            useDamage = false;
            yield return new WaitForSeconds(_damageCooldown);
            useDamage = true;
            _damageCooldownCoroutine = null;
        }
    }
}
