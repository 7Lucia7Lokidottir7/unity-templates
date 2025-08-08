using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.InventorySystem
{
    public class InventoryInteraction : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;

        public InputActionReference upAction;
        public InputActionReference downAction;
        public InputActionReference leftAction;
        public InputActionReference rightAction;
        public InputActionReference useAction;
        public InputActionReference dropAction;
        public InputActionReference switchViewAction;
        public InputActionReference dragDropAction; // Одна кнопка для drag&drop на геймпаде

        private bool isDragging = false;

        private void OnEnable()
        {
            upAction.action.performed += OnUp;
            downAction.action.performed += OnDown;
            leftAction.action.performed += OnLeft;
            rightAction.action.performed += OnRight;
            useAction.action.performed += OnUse;
            dropAction.action.performed += OnDrop;
            switchViewAction.action.performed += OnSwitchView;
            dragDropAction.action.performed += OnDragDrop;

            EnableAll();
        }
        private void OnDisable()
        {
            upAction.action.performed -= OnUp;
            downAction.action.performed -= OnDown;
            leftAction.action.performed -= OnLeft;
            rightAction.action.performed -= OnRight;
            useAction.action.performed -= OnUse;
            dropAction.action.performed -= OnDrop;
            switchViewAction.action.performed -= OnSwitchView;
            dragDropAction.action.performed -= OnDragDrop;

            DisableAll();
        }
        void EnableAll()
        {
            upAction.action.Enable(); downAction.action.Enable();
            leftAction.action.Enable(); rightAction.action.Enable();
            useAction.action.Enable(); dropAction.action.Enable();
            switchViewAction.action.Enable(); dragDropAction.action.Enable();
        }
        void DisableAll()
        {
            upAction.action.Disable(); downAction.action.Disable();
            leftAction.action.Disable(); rightAction.action.Disable();
            useAction.action.Disable(); dropAction.action.Disable();
            switchViewAction.action.Disable(); dragDropAction.action.Disable();
        }
        void OnUp(InputAction.CallbackContext ctx) => _inventory.MoveSelection(InventoryNavigationDirection.Up);
        void OnDown(InputAction.CallbackContext ctx) => _inventory.MoveSelection(InventoryNavigationDirection.Down);
        void OnLeft(InputAction.CallbackContext ctx) => _inventory.MoveSelection(InventoryNavigationDirection.Left);
        void OnRight(InputAction.CallbackContext ctx) => _inventory.MoveSelection(InventoryNavigationDirection.Right);

        void OnUse(InputAction.CallbackContext ctx)
        {
            _inventory.UseSelected();
        }

        void OnDrop(InputAction.CallbackContext ctx) => _inventory.RemoveSelected();
        void OnSwitchView(InputAction.CallbackContext ctx)
        {
            var newMode = _inventory.viewMode == InventoryViewMode.Line
                ? InventoryViewMode.Grid
                : InventoryViewMode.Line;
            _inventory.SwitchView(newMode);
        }
        void OnDragDrop(InputAction.CallbackContext ctx)
        {
            if (!isDragging) { _inventory.StartGamepadDrag(); isDragging = true; }
            else { _inventory.DropGamepadDrag(); isDragging = false; }
        }
    }
}
