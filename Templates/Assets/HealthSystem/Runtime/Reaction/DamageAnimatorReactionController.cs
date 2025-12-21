using UnityEngine;

namespace PG.HealthSystem
{
    public class DamageAnimatorReactionController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _damageTrigger = "Damage";
        [SerializeField] private GameObject _damagableObject;
        public IDamagable damagable;
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
        private void OnEnable()
        {
            damagable.damaged += OnDamage;
        }
        private void OnDisable()
        {
            damagable.damaged -= OnDamage;
        }
        void OnDamage(float damage)
        {
            _animator?.SetTrigger(_damageTrigger);
        }
    }
}
