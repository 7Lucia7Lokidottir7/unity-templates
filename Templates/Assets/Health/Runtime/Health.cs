using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PG.HealthSystem
{
    public class Health : MonoBehaviour, IDamagable, IHealable
    {
        [Header("UI")]
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private RectTransform _hitRect;
        private Coroutine _hitCoroutine;
        [SerializeField] private float _targetTransitionDuration = 0.1f;

        [Header("Cooldown")]
        [SerializeField] private bool _cooldownEnable;
        [SerializeField] private float _damageCooldown = 0.2f;
        private Coroutine _damageCooldownCoroutine;

        [Space(10)]
        [SerializeField] private GameObject _deathObject;

        public event Action<float> damaged;
        public event Action<float> healed;

        private void OnDestroy()
        {
            StopCoroutine(_hitCoroutine);
        }
        public void Setup(float maxValue, float value = 0)
        {
            _healthSlider.value = value <= 0 ? maxValue : value;
            _healthSlider.maxValue = maxValue;
        }

        public void OnDamage(float damage)
        {
            if (_cooldownEnable)
            {
                if (_damageCooldownCoroutine != null)
                {
                    return;
                }
                _damageCooldownCoroutine = StartCoroutine(OnDamageCooldown());
            }

            _healthSlider.value -= damage;
            damaged?.Invoke(damage);
            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
            }
            _hitCoroutine = StartCoroutine(HitAnimationSlider());
            if (_healthSlider.value == _healthSlider.minValue)
            {
                if (_deathObject.TryGetComponent(out IDeath death))
                {
                    death?.OnDeath();
                }
            }
        }
        IEnumerator HitAnimationSlider()
        {
            for (float i = 0; i < _targetTransitionDuration; i+= Time.deltaTime)
            {
                _hitRect.anchorMax = Vector2.Lerp(Vector2.one, Vector2.up, i / _targetTransitionDuration);
                yield return null;
            }
            _hitCoroutine = null;
        }
        
        IEnumerator OnDamageCooldown()
        {
            yield return new WaitForSeconds(_damageCooldown);
            _damageCooldownCoroutine = null;
        }

        public void OnHeal(float value)
        {
            _healthSlider.value += value;
            healed?.Invoke(value);
        }
    }
}
