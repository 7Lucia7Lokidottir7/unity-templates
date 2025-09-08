using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
namespace PG.InteractSystem
{
    public class InteractObjectTrigger : MonoBehaviour, IInteractable
    {
        public event System.Action interacted;
        private const string _PLAYER_TAG = "Player";

        public InputActionProperty interactProperty;
        private InputAction _interactAction;
        [field: SerializeField] public UnityEvent interactEvent { get; set; }
        private void Awake()
        {
            _interactAction = InputSystem.actions.FindAction(interactProperty.reference.name);
        }
        private void OnDisable()
        {
            _interactAction.performed -= OnInteract;
        }
        private void OnEnable()
        {
            _interactAction.performed += OnInteract;
        }
        public void OnInteract()
        {
            interacted?.Invoke();
            interactEvent?.Invoke();

            IInteractable.isInteractActive = false;
            IInteractable.visibleInteracted?.Invoke(false);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Debug.Log("Interact");
            OnInteract();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_PLAYER_TAG))
            {
                IInteractable.isInteractActive = true;
                IInteractable.visibleInteracted?.Invoke(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_PLAYER_TAG))
            {
                IInteractable.isInteractActive = false;
                IInteractable.visibleInteracted?.Invoke(false);
            }
        }
    }
}
