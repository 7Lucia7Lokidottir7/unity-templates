using System;
using UnityEngine;
using UnityEngine.UI;

namespace PG.HealthSystem
{
    public class GameOver : MonoBehaviour, IDeath
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _transitionDuration = 0.25f;
        [SerializeField] private Selectable _targetSelectable;

        public event Action dead;

        public void OnDeath()
        {
            _canvasGroup.gameObject.SetActive(true);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _targetSelectable.Select();
            Time.timeScale = 0;
            for (float i = 0; i < _transitionDuration; i+= Time.unscaledDeltaTime)
            {
                _canvasGroup.alpha = Mathf.Lerp(0, 1f, i / _transitionDuration);
            }
            _canvasGroup.alpha = 1f;
        }
    }
}
