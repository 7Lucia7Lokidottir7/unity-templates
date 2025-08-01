using UnityEngine;
using UnityEngine.InputSystem;
namespace PG.LocomotionSystem
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField] private float jumpPower = 7.5f;
        [SerializeField] private InputActionProperty _jumpActionProperty;
        private InputAction _jumpAction;
        [SerializeField] private GravityCharacter _gravityCharacter;
        public int airJumpMax = 0;
        private int _currentAirJump;

        private void Start()
        {
            _jumpAction = InputSystem.actions.FindAction(_jumpActionProperty.reference.name);
        }
        private void OnEnable()
        {
            _jumpAction.performed += OnJump;
        }
        private void OnDisable()
        {
            _jumpAction.performed -= OnJump;
        }
        void OnJump(InputAction.CallbackContext context)
        {
            if (_gravityCharacter.isGrounded)
            {
                _gravityCharacter.verticalVelocity = jumpPower;
            }
            else
            {
                if (airJumpMax > 0 && _currentAirJump < airJumpMax)
                {
                    _gravityCharacter.verticalVelocity = jumpPower;
                    _currentAirJump++;
                }
            }
        }
    }
}
