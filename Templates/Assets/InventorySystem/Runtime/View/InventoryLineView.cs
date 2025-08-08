using UnityEngine;

namespace PG.InventorySystem
{
    public class InventoryLineView : IInventoryView
    {
        public void DisplayItems(InventoryItem[] items, int selectedID, ItemCell[] cells)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].SetItem(items[i]);
                cells[i].SetSelected(i == selectedID);
            }
        }
        public int GetNextID(int currentID, InventoryNavigationDirection dir, int total, int columns = 1)
        {
            switch (dir)
            {
                case InventoryNavigationDirection.Left: return Mathf.Max(0, currentID - 1);
                case InventoryNavigationDirection.Right: return Mathf.Min(total - 1, currentID + 1);
                default: return currentID;
            }
        }
    }
}
