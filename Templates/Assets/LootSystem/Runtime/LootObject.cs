using UnityEngine;

namespace PG.RandomLoot
{
    using PG.InteractSystem;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;
    public class LootObject : MonoBehaviour, IInteractable
    {
        [field:SerializeField] public UnityEvent interactEvent { get; set; }

        public event System.Action interacted;

        public void OnInteract()
        {
            interactEvent?.Invoke();
            interacted?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            OnInteract();
        }
    }
}
