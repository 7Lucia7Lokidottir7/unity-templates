using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace PG.ShootSystem
{
    public class ThirdPersonAim : InputAim
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _transitionTime = 0.1f;
        private Coroutine _coroutine;
        [SerializeField] private CinemachineCamera _baseCamera;
        [SerializeField] private CinemachineCamera _targetCamera;
        public override void OnAim(bool value)
        {
            _baseCamera.gameObject.SetActive(!value);
            _targetCamera.gameObject.SetActive(value);

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(OnAimEnumerator(value));
        }
        IEnumerator OnAimEnumerator(bool value)
        {
            float baseAlpha = _canvasGroup.alpha;
            for (float i = 0; i < _transitionTime; i+= Time.unscaledDeltaTime)
            {
                _canvasGroup.alpha = Mathf.Lerp(baseAlpha, 1f, i / _transitionTime);
                yield return null;
            }
            _canvasGroup.alpha = value ? 1f : 0f;
        }
    }
}
