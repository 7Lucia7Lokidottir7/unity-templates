using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.ShootSystem
{
    public abstract class InputAim : MonoBehaviour
    {
        [SerializeField] protected InputActionProperty _aimProperty;
        private bool _isAim;
        public bool isAim 
        { 
            get => _isAim;
            set
            {
                _isAim = value;
                OnAim(isAim);
                aimed?.Invoke(_isAim);

                _animator.SetBool(_aimParameter, _isAim);
            }
        }
        public event System.Action<bool> aimed;

        [SerializeField] private Animator _animator;
        [SerializeField] private string _aimParameter = "Aim";
        private void OnEnable()
        {
            _aimProperty.action.performed += OnAim;
        }

        private void OnDisable()
        {
            _aimProperty.action.performed -= OnAim;
        }

        // Update is called once per frame
        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isAim = true;
            }
            if (context.canceled)
            {
                isAim = false;
            }
        }
        public abstract void OnAim(bool value);
    }
}
