using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using PG.HealthSystem;
namespace PG.BattleSystem
{
    public class DashController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _dashParameter = "Dash";

        [SerializeField] private float _duration = 0.1f;

        [SerializeField] private InputActionProperty _dashProperty;
        [SerializeField] private InputActionProperty _moveProperty;
        [SerializeField] private Health _health;

        private InputAction _dashAction;
        private InputAction _moveAction;

        private Coroutine _coroutine;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _dashAction = InputSystem.actions.FindAction(_dashProperty.reference.name);
            _moveAction = InputSystem.actions.FindAction(_moveProperty.reference.name);
        }

        private void OnEnable()
        {
            _dashAction.performed += OnDash;
        }

        private void OnDisable()
        {
            _dashAction.performed -= OnDash;
        }
        // Update is called once per frame
        void OnDash(InputAction.CallbackContext context)
        {
            if (_moveAction.ReadValue<float>() > 0.1f && _coroutine == null)
            {
                _animator.SetTrigger(_dashParameter);
                _coroutine = StartCoroutine(OnDashing());
            }
        }
        IEnumerator OnDashing()
        {
            _health.useDamage = false;
            yield return new WaitForSeconds(_duration);
            _health.useDamage = true;
        }
    }
}
