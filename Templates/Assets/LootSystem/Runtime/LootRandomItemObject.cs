using UnityEngine;

namespace PG.RandomLoot
{
    using PG.InteractSystem;
    using PG.InventorySystem;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    public class LootRandomItemObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private Inventory _inventory;
        [field:SerializeField] public UnityEvent interactEvent { get; set; }

        public event System.Action interacted;
        [SerializeField] private Item[] _items;
        [SerializeField, Min(1)] private int _count = 1;

        public void OnInteract()
        {
            if (_count > 1)
            {
                for (int i = 0; i < _count; i++)
                {
                    _inventory.AddItem(_items[Random.Range(0, _items.Length)]);
                }
            }
            else
            {
                _inventory.AddItem(_items[Random.Range(0, _items.Length)]);
            }
            interactEvent?.Invoke();
            interacted?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            OnInteract();
        }
    }
}
