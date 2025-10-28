using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.HealthSystem
{
    public class HealthHeartsAnimator : MonoBehaviour, IDamagable, IHealable
    {
        [Header("Hearts / UI")]
        [SerializeField] private List<Animator> _hearts = new List<Animator>(); // you can seed starting ones from the scene
        [SerializeField] private float _heartCapacity = 1f;
        [SerializeField] private string _fillParam = "fill";
        [SerializeField] private string _hitTrigger = "hit";
        [SerializeField] private string _healTrigger = "heal";

        [Tooltip("Prefab of a single heart with an Animator component")]
        [SerializeField] private GameObject _heartPrefab;

        [Tooltip("Parent transform where hearts will be instantiated")]
        [SerializeField] private Transform _heartsParent;

        [Tooltip("Automatically add new hearts when max health increases")]
        [SerializeField] private bool _autoExpandHearts = true;

        [Header("Animation")]
        [SerializeField] private float _unitsPerSecond = 20f;

        [Header("Damage Cooldown")]
        [SerializeField] private bool _cooldownEnable;
        [SerializeField] private float _damageCooldown = 0.2f;

        [Header("Gameplay")]
        public bool useDamage = true;
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

        /// <summary>
        /// Initializes the system. If value <= 0, fills health to max.
        /// Automatically creates or adjusts hearts according to maxValue.
        /// </summary>
        public void Setup(float maxValue, float value = 0)
        {
            _maxValue = Mathf.Max(0f, maxValue);
            _value = value <= 0 ? _maxValue : Mathf.Clamp(value, 0f, _maxValue);
            _target = _value;

            EnsureHeartCount();
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

            if (_target <= 0f && _deathObject != null && _deathObject.TryGetComponent(out IDeath death))
                death.OnDeath();
        }

        public void OnHeal(float value)
        {
            _target = Mathf.Clamp(_target + Mathf.Abs(value), 0f, _maxValue);
            healed?.Invoke(value);
            TriggerAll(_healTrigger);
            EnsureAnimator();
        }

        /// <summary>
        /// Increases maximum health. Optionally fills the new hearts.
        /// </summary>
        public void IncreaseMax(float delta, bool fillNewHearts = true)
        {
            if (delta <= 0f) return;
            float oldMax = _maxValue;
            _maxValue += delta;

            if (_autoExpandHearts) EnsureHeartCount();

            if (fillNewHearts)
            {
                // Add to current target as much as max increased (but not over max)
                float grow = _maxValue - oldMax;
                _target = Mathf.Clamp(_target + grow, 0f, _maxValue);
                EnsureAnimator();
            }
            else
            {
                // Just clamp to new range
                _target = Mathf.Clamp(_target, 0f, _maxValue);
                UpdateHearts(_value);
            }
        }

        /// <summary>
        /// Force set a new max health value. Optionally rebuilds all hearts.
        /// </summary>
        public void SetMax(float newMax, bool rebuildHearts = true, bool keepCurrentFill = true)
        {
            newMax = Mathf.Max(0f, newMax);
            float fillRatio = _maxValue > 0f ? _value / _maxValue : 1f;

            _maxValue = newMax;

            if (rebuildHearts) EnsureHeartCount();

            if (keepCurrentFill)
            {
                _value = _target = Mathf.Clamp(fillRatio * _maxValue, 0f, _maxValue);
            }
            else
            {
                _value = _target = Mathf.Clamp(_value, 0f, _maxValue);
            }

            UpdateHearts(_value);
        }

        /// <summary>
        /// Manually adds one heart (UI only, does not affect _maxValue).
        /// Useful if you want to pre-generate some hearts.
        /// </summary>
        public void AddOneHeart()
        {
            CreateHeart();
            UpdateHearts(_value);
        }

        /// <summary>
        /// Removes one heart from the UI (does not change _maxValue).
        /// </summary>
        public void RemoveOneHeart()
        {
            if (_hearts == null || _hearts.Count == 0) return;
            var last = _hearts[_hearts.Count - 1];
            _hearts.RemoveAt(_hearts.Count - 1);
            if (last != null) Destroy(last.gameObject);
            UpdateHearts(_value);
        }

        // ----------------------- Internal Logic -----------------------

        private void EnsureAnimator()
        {
            if (_animRoutine == null) _animRoutine = StartCoroutine(AnimateToTarget());
        }

        private IEnumerator AnimateToTarget()
        {
            while (!Mathf.Approximately(_value, _target))
            {
                float step = _unitsPerSecond * Time.unscaledDeltaTime;
                _value = (_value < _target) ? Mathf.Min(_value + step, _target) : Mathf.Max(_value - step, _target);
                UpdateHearts(_value);
                yield return null;
            }
            _animRoutine = null;
        }

        private void UpdateHearts(float value)
        {
            if (_hearts == null || _hearts.Count == 0 || _heartCapacity <= 0f) return;

            for (int i = 0; i < _hearts.Count; i++)
            {
                float v = value - i * _heartCapacity;
                float fill = Mathf.Clamp01(v / _heartCapacity);
                if (_hearts[i] != null)
                    _hearts[i].SetFloat(_fillParam, fill);
            }
        }

        private void TriggerAll(string trigger)
        {
            if (string.IsNullOrEmpty(trigger) || _hearts == null) return;
            for (int i = 0; i < _hearts.Count; i++)
            {
                if (_hearts[i] == null) continue;
                _hearts[i].ResetTrigger(trigger);
                _hearts[i].SetTrigger(trigger);
            }
        }

        private IEnumerator DamageCooldown()
        {
            useDamage = false;
            yield return new WaitForSeconds(_damageCooldown);
            useDamage = true;
            _damageCooldownCoroutine = null;
        }

        /// <summary>
        /// Ensures that the number of hearts matches the current _maxValue
        /// based on heart capacity. Creates or removes hearts as needed.
        /// </summary>
        private void EnsureHeartCount()
        {
            if (_heartCapacity <= 0f) return;

            int required = Mathf.CeilToInt(_maxValue / _heartCapacity);
            if (required < 0) required = 0;

            // Expand if needed
            while (_hearts.Count < required)
                CreateHeart();

            // Shrink if needed
            while (_hearts.Count > required)
            {
                var last = _hearts[_hearts.Count - 1];
                _hearts.RemoveAt(_hearts.Count - 1);
                if (last != null) Destroy(last.gameObject);
            }
        }

        /// <summary>
        /// Instantiates a new heart from the prefab and adds it to the list.
        /// </summary>
        private void CreateHeart()
        {
            if (_heartPrefab == null)
            {
                Debug.LogWarning("[HealthHeartsAnimator] Heart prefab is null — cannot create a new heart.");
                return;
            }

            Transform parent = _heartsParent != null ? _heartsParent : transform;
            var go = Instantiate(_heartPrefab, parent);
            var animator = go.GetComponent<Animator>();
            if (animator == null)
            {
                animator = go.GetComponentInChildren<Animator>();
            }

            if (animator == null)
            {
                Debug.LogWarning("[HealthHeartsAnimator] Heart prefab has no Animator — please add one.");
                Destroy(go);
                return;
            }

            _hearts.Add(animator);

            // Initialize the fill param to 0 to avoid flashing
            animator.SetFloat(_fillParam, 0f);
        }
    }
}
