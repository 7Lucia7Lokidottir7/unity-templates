using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.MenuManagement
{
    public class CursorVisibleMenuChanger : MonoBehaviour
    {
        private void OnDisable()
        {
            HideCursor();
        }
        private void LateUpdate()
        {
            // Проверка движения мыши
            if (Mouse.current.delta.ReadValue() != Vector2.zero)
            {
                ShowCursor();
            }

            // Можно также добавить проверку ввода с геймпада для скрытия курсора
            if (Gamepad.current != null && Gamepad.current.IsActuated())
            {
                HideCursor();
            }
        }

        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
