using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorVisibleMenuChanger : MonoBehaviour
{
    private void OnDisable()
    {
        HideCursor();
    }
    private void LateUpdate()
    {
        // �������� �������� ����
        if (Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            ShowCursor();
        }

        // ����� ����� �������� �������� ����� � �������� ��� ������� �������
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
