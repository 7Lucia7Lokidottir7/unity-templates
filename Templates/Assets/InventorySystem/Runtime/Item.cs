using UnityEngine;

namespace PG.InventorySystem
{
    [CreateAssetMenu(menuName = "PG/Inventory/Item")]
    public class Item : ScriptableObject
    {
        public string nameItem;
        public string descriptionItem;
        public Sprite iconItem;
        public Sprite largeIconItem;
        public bool isStackable;
        public bool isDropAfterInteract;
        public virtual void Use(Inventory inventory, int slot)
        {
            Debug.Log($"{nameItem} used!");
            // По умолчанию ничего не делает, но потом — расширишь: хил, экипировка и т.д.
        }

    }
}
