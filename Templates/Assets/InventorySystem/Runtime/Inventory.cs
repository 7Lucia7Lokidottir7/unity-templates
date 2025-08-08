using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace PG.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [Header("Config")]
        public InventoryViewMode viewMode = InventoryViewMode.Grid;
        public int columns = 4;

        [Header("DB")]
        public ItemContainer itemContainer; // тво€ база данных!

        [Header("UI")]
        public Transform cellContainer;
        public AssetReferenceGameObject addressableCell; // label/key префаба!
        public Image largeIconPanel;
        public TMP_Text descriptionPanel;

        private IInventoryView _lineView = new InventoryLineView();
        private IInventoryView _gridView = new InventoryGridView();
        private IInventoryView _currentView;

        private InventoryItem[] _items;
        private ItemCell[] _cells;
        private int? dragSourceID = null;
        public int selectedID { get; private set; }
        public int Count => _items.Length;

        public InventoryItem GetSelected() => _items[selectedID];

        [SerializeField] private string _fileName = "Inventory.json";
        private string SavePath => Path.Combine(Application.persistentDataPath, _fileName);
        private InventorySaveData _saveData = new InventorySaveData();

        public System.Action OnReady; // событие когда €чейки созданы

        private void Awake()
        {
            CreateCellsAsync(); 
            Load();
        }

        public void Save()
        {
            _saveData.items.Clear();
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] != null && _items[i].item != null)
                {
                    var slot = new InventorySaveData.ItemSlot();
                    slot.itemId = itemContainer.Items.IndexOf(_items[i].item);
                    slot.count = _items[i].count;
                    _saveData.items.Add(slot);
                }
            }
            string json = JsonUtility.ToJson(_saveData, true);
            File.WriteAllText(SavePath, json);
        }

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                Display(); // чтобы пустой инвентарь сразу отобразилс€
                return;
            }

            string json = File.ReadAllText(SavePath);
            JsonUtility.FromJsonOverwrite(json, _saveData);

            for (int i = 0; i < _items.Length; i++)
                _items[i] = null;

            for (int i = 0; i < _saveData.items.Count && i < _items.Length; i++)
            {
                var slot = _saveData.items[i];
                if (slot.itemId >= 0 && slot.itemId < itemContainer.Items.Count)
                {
                    _items[i] = new InventoryItem(itemContainer.Items[slot.itemId], slot.count);
                }
            }
            Display();
        }
        // јсинхронно создаЄм €чейки через Addressables!
        private void CreateCellsAsync()
        {
            int cellCount = columns * columns;
            _items = new InventoryItem[cellCount];
            _cells = new ItemCell[cellCount];

            int created = 0;
            for (int i = 0; i < cellCount; i++)
            {
                Addressables.InstantiateAsync(addressableCell, cellContainer).Completed += (op) =>
                {
                    var cell = op.Result.GetComponent<ItemCell>();
                    int id = created;
                    cell.Init(this, id);
                    _cells[id] = cell;
                    _items[id] = null;
                    created++;
                    if (created == cellCount)
                    {
                        SwitchView(viewMode);
                        Display();
                        OnReady?.Invoke();
                    }
                };
            }
        }

        public void SwitchView(InventoryViewMode mode)
        {
            viewMode = mode;
            _currentView = (mode == InventoryViewMode.Grid) ? (IInventoryView)_gridView : _lineView;
            Display();
        }
        public void Display()
        {
            _currentView?.DisplayItems(_items, selectedID, _cells);
            var selected = _items[selectedID];
            largeIconPanel.sprite = (selected != null && selected.item != null) ? selected.item.largeIconItem : null;
            descriptionPanel.text = (selected != null && selected.item != null) ? selected.item.descriptionItem : "";
        }
        public void MoveSelection(InventoryNavigationDirection dir)
        {
            int next = _currentView.GetNextID(selectedID, dir, _items.Length, columns);
            if (next != selectedID)
            {
                selectedID = next;
                Display();
            }
        }
        public void SelectCell(int id)
        {
            selectedID = id;
            Display();
        }
        public void AddItem(Item item, int count = 1)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i]?.item == item && item.isStackable)
                {
                    _items[i].count += count;
                    Display();
                    return;
                }
            }
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null || _items[i].item == itemContainer.emptyItem)
                {
                    _items[i] = new InventoryItem(item, count);
                    Display();
                    return;
                }
            }
        }
        public void RemoveSelected()
        {
            if (_items[selectedID] != null)
            {
                _items[selectedID].count--;
                if (_items[selectedID].count <= 0) _items[selectedID] = null;
                Display();
            }
        }
        // --- DRAG&DROP ---
        public void StartDrag(int sourceID) { dragSourceID = sourceID; }
        public void EndDrag() { dragSourceID = null; }
        public void DropTo(int targetID)
        {
            if (dragSourceID.HasValue && dragSourceID.Value != targetID)
            {
                var tmp = _items[targetID];
                _items[targetID] = _items[dragSourceID.Value];
                _items[dragSourceID.Value] = tmp;
                Display();
            }
            dragSourceID = null;
        }
        // Drag между двум€ Inventory
        public void DropToOtherInventory(Inventory other, int fromID, int toID)
        {
            var myItem = _items[fromID];
            var otherItem = other._items[toID];

            _items[fromID] = otherItem;
            other._items[toID] = myItem;

            Display();
            other.Display();
        }
        // ƒл€ сундука Ч выгрузка
        public InventoryItem TakeItem(int index)
        {
            var item = _items[index];
            _items[index] = null;
            Display();
            return item;
        }
        public void PlaceItem(int index, InventoryItem item)
        {
            _items[index] = item;
            Display();
        }
        public void UseSelected()
        {
            var item = _items[selectedID];
            if (item != null && item.item != null)
            {
                item.item.Use(this, selectedID);

                // ≈сли предмет одноразовый Ч уменьшаем количество
                if (item.item.isDropAfterInteract)
                {
                    RemoveSelected();
                }
                else
                {
                    Save();
                    Display();
                }
            }
        }
        // --- ƒл€ геймпада ---
        public void StartGamepadDrag() { dragSourceID = selectedID; }
        public void DropGamepadDrag()
        {
            if (dragSourceID.HasValue && dragSourceID.Value != selectedID)
                DropTo(selectedID);
            dragSourceID = null;
        }
        // —охранение и загрузка работают через индексы itemContainer.Items!
        // (ѕримени методы Save/Load из прошлых примеров, только используй itemContainer.Items)

        public InventoryItem GetItem(int index)
        {
            if (index < 0 || index >= _items.Length) return null;
            return _items[index];
        }

        public void RemoveItemAt(int index)
        {
            if (index < 0 || index >= _items.Length) return;
            _items[index] = null;
            Display();
        }

    }
}
