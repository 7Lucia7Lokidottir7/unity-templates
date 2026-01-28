using System;
using UnityEngine;
using UnityEngine.Events;

namespace PG.HealthSystem
{
    public class DamagableLinker : MonoBehaviour, IDamagable
    {
        [SerializeField] private GameObject _damagableObject;
        public IDamagable damagable;

        [SerializeField] private UnityEvent _damageEvent;
        public event Action<float, GameObject> damaged;

        private void OnValidate()
        {
            if (_damagableObject != null && _damagableObject.GetComponent<IDamagable>() == null)
            {
                _damagableObject = null;
                Debug.LogError("DamagableObject haven't interface IDamagable!");
            }
        }
        private void Awake()
        {
            _damagableObject.TryGetComponent(out damagable);
        }

        public void OnDamage(float damage, bool ignoreDamage = false, GameObject damageObject = null)
        {
            damagable?.OnDamage(damage, ignoreDamage);
            _damageEvent?.Invoke();
            damaged.Invoke(damage, damageObject);
        }
    }
}
