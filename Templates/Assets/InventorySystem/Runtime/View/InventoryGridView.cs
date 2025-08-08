using UnityEngine;

namespace PG.InventorySystem
{
    public class InventoryGridView : IInventoryView
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
            int row = currentID / columns;
            int col = currentID % columns;
            int rows = Mathf.CeilToInt((float)total / columns);

            switch (dir)
            {
                case InventoryNavigationDirection.Up: row = Mathf.Max(0, row - 1); break;
                case InventoryNavigationDirection.Down: row = Mathf.Min(rows - 1, row + 1); break;
                case InventoryNavigationDirection.Left: col = Mathf.Max(0, col - 1); break;
                case InventoryNavigationDirection.Right: col = Mathf.Min(columns - 1, col + 1); break;
            }
            int newID = row * columns + col;
            if (newID >= total) newID = currentID;
            return newID;
        }
    }
}
