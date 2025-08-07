using UnityEngine;

namespace PG.RandomLoot
{
    using PG.InteractSystem;
    using System;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;
    public class LootObject : MonoBehaviour, IInteractable
    {
        [field:SerializeField] public UnityEvent interactEvent { get; set; }

        public event Action interacted;

        public void OnInteract()
        {
            interactEvent?.Invoke();
            interacted?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            OnInteract();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
