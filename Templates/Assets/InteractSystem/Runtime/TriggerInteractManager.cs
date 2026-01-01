using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.InteractSystem
{
    public class TriggerInteractManager : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _interactProperty;
        private List<IInteractable> _interactables = new List<IInteractable>();
        public event System.Action<bool> visibled;
        public static TriggerInteractManager instance;
        private void Awake()
        {
            instance = this;
        }
        private void OnEnable()
        {
            _interactProperty.action.performed += Interact;
        }
        private void OnDisable()
        {
            _interactProperty.action.performed -= Interact;
        }
        void Interact(InputAction.CallbackContext context)
        {
            if (_interactables.Count > 0)
            {
                for (int i = _interactables.Count - 1; i >= 0; i--)
                {
                    _interactables[i].OnInteract();
                    break;
                }
            }
        }
        public void AddTrigger(IInteractable interactable)
        {
            if(_interactables.Count == 0)
            {
                visibled?.Invoke(true);
            }

            _interactables.Add(interactable);
        }
        public void RemoveTrigger(IInteractable interactable)
        {
            _interactables.Remove(interactable);

            if (_interactables.Count == 0)
            {
                visibled?.Invoke(false);
            }
        }
    }
}
