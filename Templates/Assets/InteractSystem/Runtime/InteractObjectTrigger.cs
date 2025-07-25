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
        [field:SerializeField]public UnityEvent interactEvent { get; set; }

        public void OnInteract()
        {
            interacted?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
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
