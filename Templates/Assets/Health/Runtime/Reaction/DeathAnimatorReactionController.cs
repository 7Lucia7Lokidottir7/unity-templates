using UnityEngine;

namespace PG.HealthSystem
{
    public class DeathAnimatorReactionController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _deathState = "Base Layer.Death";
        private int _hashState;
        [SerializeField] private float _transitionAnimation = 0.25f;

        [SerializeField] private GameObject _deathObject;
        public IDeath death;

        private void OnValidate()
        {
            if (_deathObject.GetComponent<IDamagable>() == null)
            {
                _deathObject = null;
                Debug.LogError("DamagableObject haven't interface IDamagable!");
            }
        }
        private void Awake()
        {
            _hashState = Animator.StringToHash(_deathState);
            _deathObject.TryGetComponent(out death);
        }
        private void OnEnable()
        {
            death.dead += OnDead;
        }
        private void OnDisable()
        {
            death.dead -= OnDead;
        }
        void OnDead()
        {
            _animator?.CrossFadeInFixedTime(_hashState, _transitionAnimation);
        }
    }
}
