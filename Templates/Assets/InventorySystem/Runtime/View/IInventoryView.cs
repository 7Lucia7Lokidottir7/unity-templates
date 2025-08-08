namespace PG.InventorySystem
{
    public interface IInventoryView
    {
        void DisplayItems(InventoryItem[] items, int selectedID, ItemCell[] cells);
        int GetNextID(int currentID, InventoryNavigationDirection dir, int total, int columns = 1);
    }
}
