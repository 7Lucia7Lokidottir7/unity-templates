using UnityEngine;
namespace PG.LocomotionSystem
{
    public class GravityCharacter : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private Vector3 _groundCheckOffset;
        [SerializeField] private LayerMask _groundLayer;
        private float _gravityWeight = -9.81f;
        public float verticalVelocity;
        private bool _isGrounded;
        public bool isGrounded { get => _isGrounded;
            private set
            {
                _isGrounded = value;
                grounded?.Invoke(_isGrounded);
            }
        }
        public event System.Action<bool> grounded;
        private void FixedUpdate()
        {
            if (!_isGrounded)
            {
                verticalVelocity -= 1 * _gravityWeight * Time.fixedDeltaTime;
            }
            else
            {
                verticalVelocity = 0;
            }
            _characterController.Move(Vector3.up * verticalVelocity * Time.fixedDeltaTime);
            GroundCheck();
        }
        void GroundCheck()
        {
            isGrounded = Physics.CheckSphere(transform.TransformPoint(_groundCheckOffset), _groundCheckRadius, _groundLayer);
        }
    }
}
