using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.ShootSystem
{
    public abstract class InputShooter : MonoBehaviour
    {
        [SerializeField] private InputActionProperty shootProperty;
        private InputAction _shootAction;
        public bool isShootEnable = true;
        public bool isAuto;

        public float delayShoot = 0.025f;

        public event System.Action<bool> shooted;
        public event System.Action<bool> autoShooted;

        private Coroutine _coroutine;

        private void Awake()
        {
            _shootAction = InputSystem.actions.FindAction(shootProperty.reference.name);
        }

        private void OnEnable()
        {
            _shootAction.performed += Shoot;
        }

        private void OnDisable()
        {
            _shootAction.performed -= Shoot;
        }

        // Update is called once per frame
        void Shoot(InputAction.CallbackContext context)
        {
            if (isShootEnable)
            {
                if (isAuto)
                {
                    if (context.started) {
                        _coroutine = StartCoroutine(ShootEnumerator());
                    }
                    if (context.canceled)
                    {
                        if (_coroutine != null)
                        {
                            StopCoroutine(_coroutine);
                        }
                    }
                }
                else
                {
                    shooted?.Invoke(isShootEnable);
                    OnShoot();
                }
            }
        }
        IEnumerator ShootEnumerator()
        {
            shooted?.Invoke(isShootEnable);
            while (isShootEnable)
            {
                autoShooted?.Invoke(true);
                OnShoot();
                yield return new WaitForSeconds(delayShoot);
            }
            OnShootEnd();
            autoShooted?.Invoke(false);
        }
        public abstract void OnShoot();
        public abstract void OnShootEnd();
    }
}
