using UnityEngine;

namespace PG.InventorySystem
{
    public class Chest : MonoBehaviour
    {
        private const string _PLAYER_TAG = "Player";
        [Header("Inventories")]
        public Inventory chestInventory;    // Inventory ����� �������
        public Inventory playerInventory;   // Inventory ������

        [Header("UI")]
        public GameObject chestPanel;       // UI-������ ������� (��������� �����)
        public GameObject playerPanel;      // UI-������ ��������� ������

        private bool _isOpen = false;

        public void OpenChest()
        {
            if (_isOpen) return;
            _isOpen = true;
            chestPanel.SetActive(true);
            playerPanel.SetActive(true);

            // ����� ������������ UI, ����������� �������� ������ � �.�.
            Debug.Log("Chest is open!");
        }

        public void CloseChest()
        {
            if (!_isOpen) return;
            _isOpen = false;
            chestPanel.SetActive(false);
            playerPanel.SetActive(false);

            // �������������� ���������� ������� � �.�.
            Debug.Log("Chest is closed!");
        }

        // ����� �������� ������� ����� �� ��������/������:
        private void OnTriggerEnter(Collider other)
        {
            // ��������, ���� � ���� 3D, � � ������ ��� "Player"
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
