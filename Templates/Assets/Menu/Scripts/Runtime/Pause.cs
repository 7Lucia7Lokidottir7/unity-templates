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
                if (!UIManager.RegisterClose(this.gameObject)) return;
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