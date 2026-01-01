using UnityEngine;
using UnityEngine.InputSystem;
namespace PG.InteractSystem
{
    public class CameraInteractManager : MonoBehaviour, IInteractVisible
    {
        public InputActionProperty interactProperty { get; set; }
        private Camera _camera;
        [SerializeField] private float _targetDistance = 3f;
        private Ray _ray;
        private RaycastHit _hit;


        private void Awake()
        {
            _camera = Camera.main;
        }
        private void OnEnable()
        {
            interactProperty.action.performed += OnInteract;
        }
        private void OnDisable()
        {
            interactProperty.action.performed -= OnInteract;
        }
        public void OnInteract()
        {
            if (_hit.collider != null && _hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteract();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            OnInteract();
        }
        private void FixedUpdate()
        {
            _ray = _camera.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
            if (Physics.Raycast(_ray, out _hit, _targetDistance))
            {
                if (_hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    IInteractVisible.isInteractActive = true;
                    IInteractVisible.visibleInteracted?.Invoke(true);
                }
                else
                {
                    IInteractVisible.isInteractActive = false;
                    IInteractVisible.visibleInteracted?.Invoke(false);
                }
            }
            else
            {
                IInteractVisible.isInteractActive = false;
                IInteractVisible.visibleInteracted?.Invoke(false);
            }
        }
    }
}
