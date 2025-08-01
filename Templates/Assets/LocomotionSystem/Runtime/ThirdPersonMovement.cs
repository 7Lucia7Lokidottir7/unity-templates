using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
namespace PG.LocomotionSystem
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _moveProperty;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _orientation;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _turnDuration = 4f;
        private Coroutine _coroutine;


        private void OnEnable()
        {
            _moveProperty.action.started += OnMove;
            _moveProperty.action.canceled += OnMove;
        }
        private void OnDisable()
        {
            _moveProperty.action.started -= OnMove;
            _moveProperty.action.canceled -= OnMove;
        }
        void OnMove(InputAction.CallbackContext context)
        {
            if (context.started && _coroutine == null)
            {
                _coroutine = StartCoroutine(OnMove());
            }
            if (context.canceled && _coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
        IEnumerator OnMove()
        {
            while (enabled)
            {
                Vector2 input = _moveProperty.action.ReadValue<Vector2>().normalized;
                Vector3 direction;
                float moveAmount = Mathf.Abs(input.x) + Mathf.Abs(input.y);
                var moveInput = (new Vector3(input.x, 0, input.y)).normalized;

                direction = Quaternion.Euler(0, _orientation.eulerAngles.y, 0) * moveInput;

                if (moveAmount > 0.1f)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    _characterController.transform.rotation = Quaternion.RotateTowards(_characterController.transform.rotation, lookRotation, _turnDuration * Time.deltaTime);
                }


                _characterController.Move(direction * _moveSpeed * Time.deltaTime);
                yield return null;
            }
        }


    }
}
