using UnityEngine;

namespace PG.InventorySystem
{
    public class Chest : MonoBehaviour
    {
        private const string _PLAYER_TAG = "Player";
        [Header("Inventories")]
        public Inventory chestInventory;    // Inventory этого сундука
        public Inventory playerInventory;   // Inventory игрока

        [Header("UI")]
        public GameObject chestPanel;       // UI-панель сундука (контейнер ячеек)
        public GameObject playerPanel;      // UI-панель инвентаря игрока

        private bool _isOpen = false;

        public void OpenChest()
        {
            if (_isOpen) return;
            _isOpen = true;
            chestPanel.SetActive(true);
            playerPanel.SetActive(true);

            // Можно фокусировать UI, блокировать движение игрока и т.д.
            Debug.Log("Chest is open!");
        }

        public void CloseChest()
        {
            if (!_isOpen) return;
            _isOpen = false;
            chestPanel.SetActive(false);
            playerPanel.SetActive(false);

            // Разблокировать управление игроком и т.д.
            Debug.Log("Chest is closed!");
        }

        // Можно добавить удобный вызов из триггера/кнопки:
        private void OnTriggerEnter(Collider other)
        {
            // Например, если у тебя 3D, а у игрока тег "Player"
            if (other.CompareTag(_PLAYER_TAG))
            {
                OpenChest();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_PLAYER_TAG))
            {
                CloseChest();
            }
        }
    }
}
