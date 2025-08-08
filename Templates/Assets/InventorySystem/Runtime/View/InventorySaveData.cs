using System.Collections.Generic;

[System.Serializable]
public class InventorySaveData
{
    public List<ItemSlot> items = new List<ItemSlot>();
    [System.Serializable]
    public class ItemSlot
    {
        public int itemId;
        public int count;
    }
}
