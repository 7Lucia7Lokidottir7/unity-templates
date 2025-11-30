using System;
using UnityEngine;

namespace PG.HealthSystem
{
    public class DamagableLinker : MonoBehaviour, IDamagable
    {
        [SerializeField] private GameObject _damagableObject;
        public IDamagable damagable;

        public event Action<float> damaged;

        private void OnValidate()
        {
            if (_damagableObject.GetComponent<IDamagable>() == null)
            {
                _damagableObject = null;
                Debug.LogError("DamagableObject haven't interface IDamagable!");
            }
        }
        private void Awake()
        {
            _damagableObject.TryGetComponent(out damagable);
        }

        public void OnDamage(float damage, bool ignoreDamage = false)
        {
            damagable?.OnDamage(damage, ignoreDamage);
            damaged.Invoke(damage);
        }
    }
}
