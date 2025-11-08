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
        public static bool isPause;
        public static bool isPauseControls { set; get; }
        public static bool isPauseEnable { set; get; } = true;
        private float _standardTime = 1f; 
        private bool _prePauseCursorVisible;
        private void Awake()
        {
            isPauseEnable = true;
        }
        private void OnEnable()
        {
            _key.action.performed += InputChangePause;
        }
        private void OnDisable()
        {
            _key.action.performed -= InputChangePause;
        }
        private void OnDestroy()
        {

            isPauseControls = false;
            isPause = false;
            isPauseEnable = true;
        }
        void InputChangePause(InputAction.CallbackContext context)
        {
            ChangePause();
        }
        public void ChangePause()
        {
            if (isPauseEnable)
            {
                isPause = !isPause;
                if (isPause)
                {
                    _panel.gameObject.SetActive(true);
                    _prePauseCursorVisible = Menu.CursorVeisible; // Сохраняем состояние видимости курсора перед паузой
                    _standardTime = Time.timeScale;
                    Time.timeScale = 0f;
                    Menu.OnChangeCursorVisible(true);
                    _selectButton.Select();
                    _panel.OnAlphaTween(1f, 0.25f, true);
                    _panel.interactable = true;
                    _panel.blocksRaycasts = true;
                    _panel.EnableUITween();
                }
                else
                {
                    _panel.DisableUITween();
                    Time.timeScale = _standardTime;
                    _panel.OnAlphaTween(0f, 0.25f, true, null, () =>
                    {
                        _panel.gameObject.SetActive(false);
                        Menu.OnChangeCursorVisible(false); // Восстанавливаем состояние видимости курсора после выхода из паузы
                    });
                    _panel.interactable = false;
                    _panel.blocksRaycasts = false;
                }
            }
        
        }

    }

}
