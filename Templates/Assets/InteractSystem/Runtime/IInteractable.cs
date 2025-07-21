using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PG.InteractSystem
{
    public interface IInteractable
    {
        public static bool isInteractActive;
        public void OnInteract();
        public void OnInteract(InputAction.CallbackContext context);
        public event System.Action interacted;
        public static System.Action<bool> visibleInteracted;
        public UnityEvent interactEvent { get; set; }
    }
}
