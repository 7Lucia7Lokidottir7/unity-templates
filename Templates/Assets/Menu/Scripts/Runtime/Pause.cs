using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using PG.Tween;

namespace PG.MenuManagement
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _key;
        [SerializeField] private Button _selectButton;
        [SerializeField] private CanvasGroup _panel;

        private UIShowHide _uiShowHide;

        // УБРАНО static: теперь у каждой панели своя переменная состояния
        private bool _isCurrentPanelActive;

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
            // Если объект удален, принудительно очищаем регистрацию в менеджере
            if (_isCurrentPanelActive)
            {
                UIManager.RegisterClose(_panel.gameObject);
            }
        }

        void InputChangePause(InputAction.CallbackContext context) => ChangePause();

        public void ChangePause()
        {
            if (!isPauseEnable) return;

            // Логика открытия
            if (!_isCurrentPanelActive)
            {
                // Если какая-то ДРУГАЯ панель уже открыта — игнорируем ввод
                if (UIManager.IsAnyPanelOpen) return;

                // Пытаемся занять место в UIManager
                if (UIManager.RequestOpen(_panel.gameObject))
                {
                    OpenPause();
                }
            }
            // Логика закрытия
            else
            {
                // Закрываем, только если мы сами открыты
                if (UIManager.RegisterClose(_panel.gameObject))
                {
                    ClosePause();
                }
            }
        }

        private void OpenPause()
        {
            _isCurrentPanelActive = true;
            _standardTime = Time.timeScale;
            Time.timeScale = 0f;

            Menu.OnChangeCursorVisible(true);

            if (_uiShowHide)
                _uiShowHide.Show();
            else
                _panel.gameObject.SetActive(true);

            _selectButton.Select();

            _panel.interactable = true;
            _panel.blocksRaycasts = true;
            _panel.OnAlphaTween(1f, 0.25f, true);
        }

        private void ClosePause()
        {
            _isCurrentPanelActive = false;
            Time.timeScale = _standardTime;

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