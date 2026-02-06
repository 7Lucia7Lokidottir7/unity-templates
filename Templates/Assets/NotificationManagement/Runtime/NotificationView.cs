using System;
using System.Collections;
using PG.MenuManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.NotificationManagement
{
    public class NotificationView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private UIShowHide _showHide;

        private Action<NotificationView> _returnToPoolCallback;
        private Coroutine _displayCoroutine;

        // Инициализация отображения
        public void Setup(NotificationData data, float duration, Action<NotificationView> returnCallback)
        {
            _returnToPoolCallback = returnCallback;

            _titleText.text = data.name;
            _descriptionText.text = data.description;

            if (_iconImage != null)
            {
                _iconImage.gameObject.SetActive(data.icon != null);
                _iconImage.sprite = data.icon;
            }

            // Запускаем процесс показа
            if (_displayCoroutine != null) StopCoroutine(_displayCoroutine);
            _displayCoroutine = StartCoroutine(DisplayRoutine(duration));
        }

        private IEnumerator DisplayRoutine(float duration)
        {
            _showHide.Show();

            // Ждем указанное время
            yield return new WaitForSeconds(duration);

            Hide();
        }

        public void Hide()
        {
            // После анимации скрытия вызываем колбэк для возврата в пул
            _showHide.Hide(() =>
            {
                _returnToPoolCallback?.Invoke(this);
            });
        }

        // Метод для мгновенного сброса (если нужно принудительно очистить)
        public void ForceReset()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }
}