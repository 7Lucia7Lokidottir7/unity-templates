using System.Collections.Generic;
using UnityEngine;

namespace PG.InventorySystem
{
    [CreateAssetMenu(menuName = "PG/Inventory/Item Container")]
    public class ItemContainer : ScriptableObject
    {
        [field:SerializeField] public List<Item> Items { get; private set; } = new List<Item>();
        [field:SerializeField] public Item emptyItem { get; private set; }
    }
}
