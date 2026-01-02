using UnityEngine;

namespace PG.MenuManagement
{
    public class GameObjectsStateController : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("If true, objects Disable by open menu. If false — Enable.")]
        [SerializeField] private bool _disableOnMenuOpen = true;

        [Header("Targets")]
        [SerializeField] private GameObject[] _targetObjects;

        private void OnEnable()
        {
            // Подписываемся на событие изменения состояния UI
            UIManager.OnStateChanged += HandleUIStateChanged;

            // Сразу устанавливаем актуальное состояние при включении скрипта
            HandleUIStateChanged(UIManager.IsAnyPanelOpen);
        }

        private void OnDisable()
        {
            // Отписываемся, чтобы избежать утечек памяти
            UIManager.OnStateChanged -= HandleUIStateChanged;
        }

        private void HandleUIStateChanged(bool isMenuOpen)
        {
            // Определяем, должны ли объекты быть активны
            // Если _disableOnMenuOpen = true, то при isMenuOpen = true состояние должно быть false
            bool shouldBeActive = _disableOnMenuOpen ? !isMenuOpen : isMenuOpen;

            foreach (var obj in _targetObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(shouldBeActive);
                }
            }
        }
    }
}