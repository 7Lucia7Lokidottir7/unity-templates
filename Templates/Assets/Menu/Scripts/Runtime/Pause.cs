using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using PG.Tween;

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
    public class Pause : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _key;
        [SerializeField] private Button _selectButton;
        [SerializeField] private CanvasGroup _panel;

        // Ссылка на компонент анимации, если он есть
        private UIShowHide _uiShowHide;

        public static bool isPause;
        public static bool isPauseEnable { set; get; } = true;
        private float _standardTime = 1f;

        private void Awake()
        {
            isPauseEnable = true;
            _uiShowHide = GetComponent<UIShowHide>();
        }

        private void OnEnable() => _key.action.performed += InputChangePause;
        private void OnDisable() => _key.action.performed -= InputChangePause;

        private void OnDestroy()
        {
            isPause = false;
            isPauseEnable = true;
            UIManager.RegisterClose(this.gameObject);
        }

        void InputChangePause(InputAction.CallbackContext context) => ChangePause();

        public void ChangePause()
        {
            if (!isPauseEnable) return;

            if (!isPause)
            {
                // Пытаемся открыть: если UIManager занят другой панелью, ничего не делаем
                if (!UIManager.RequestOpen(this.gameObject)) return;

                OpenPause();
            }
            else
            {
                ClosePause();
            }
        }

        private void OpenPause()
        {
            isPause = true;
            _standardTime = Time.timeScale;
            Time.timeScale = 0f;

            Menu.OnChangeCursorVisible(true);

            if (_uiShowHide)
                _uiShowHide.Show();
            else
                _panel.gameObject.SetActive(true);

            _selectButton.Select();

            // Настройка CanvasGroup
            _panel.interactable = true;
            _panel.blocksRaycasts = true;
            _panel.OnAlphaTween(1f, 0.25f, true);
        }

        private void ClosePause()
        {
            isPause = false;
            Time.timeScale = _standardTime;

            UIManager.RegisterClose(this.gameObject);

            if (_uiShowHide)
            {
                _uiShowHide.Hide(() => Menu.OnChangeCursorVisible(false));
            }
            else
            {
                _panel.OnAlphaTween(0f, 0.25f, true, null, () =>
                {
                    _panel.gameObject.SetActive(false);
                    Menu.OnChangeCursorVisible(false);
                });
            }

            _panel.interactable = false;
            _panel.blocksRaycasts = false;
        }
    }
}