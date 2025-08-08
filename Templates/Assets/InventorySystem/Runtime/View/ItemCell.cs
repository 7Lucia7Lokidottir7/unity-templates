using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace PG.InventorySystem
{
    public class ItemCell : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;
        [SerializeField] private GameObject _highlight;

        public int id { get; private set; }
        private Inventory _inventory;

        public void Init(Inventory inventory, int id)
        {
            this._inventory = inventory;
            this.id = id;
        }
        public void SetItem(InventoryItem invItem)
        {
            if (invItem != null && invItem.item != null)
            {
                _icon.sprite = invItem.item.iconItem;
                _icon.enabled = true;
                _count.text = invItem.item.isStackable ? invItem.count.ToString() : "";
            }
            else
            {
                _icon.sprite = null;
                _icon.enabled = false;
                _count.text = "";
            }
        }
        public void SetSelected(bool selected)
        {
            if (_highlight != null)
                _highlight.SetActive(selected);
        }

        // Drag&Drop мышкой
        public void OnBeginDrag(PointerEventData eventData) => _inventory?.StartDrag(id);
        public void OnEndDrag(PointerEventData eventData) => _inventory?.EndDrag();
        public void OnDrop(PointerEventData eventData)
        {
            // Если два Inventory — надо знать куда дропать
            var pointer = eventData.pointerDrag?.GetComponent<ItemCell>();
            if (pointer != null && pointer._inventory != this._inventory)
            {
                // Переместить между инвентарями
                pointer._inventory.DropToOtherInventory(_inventory, pointer.id, id);
            }
            else
            {
                _inventory?.DropTo(id);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                _inventory.SelectCell(id);
        }
    }
}
