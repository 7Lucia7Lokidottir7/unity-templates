using PG.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PG.MenuManagement
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class UIShowHide : MonoBehaviour
    {
        public enum EnterMode
        {
            FromLeft, FromRight, FromTop, FromBottom,
            ScaleIn, FadeIn, PopIn,
            FromLeftAndFade, FromRightAndFade, FromTopAndFade, FromBottomAndFade
        }
        public enum ExitMode
        {
            ToLeft, ToRight, ToTop, ToBottom,
            ScaleOut, FadeOut, PopOut,
            ToLeftAndFade, ToRightAndFade, ToTopAndFade, ToBottomAndFade
        }

        [Header("General")]
        [SerializeField] private bool _playOnEnable = true;
        [SerializeField] private bool _deactivateAfterHide = true;
        [SerializeField] private bool _lockInteractionDuringTween = true;
        [SerializeField] private bool _useIgnoreTimeScale = true;
        [Tooltip("Остановить все активные твины через PGTween перед стартом новой анимации.")]
        [SerializeField] private bool _stopExistingTweens = false;

        [Header("Timing")]
        [Min(0f)][SerializeField] private float _delayIn = 0f;
        [Min(0f)][SerializeField] private float _delayOut = 0f;
        [Min(0.01f)][SerializeField] private float _durationIn = 0.35f;
        [Min(0.01f)][SerializeField] private float _durationOut = 0.30f;

        [Header("Easing")]
        [SerializeField] private AnimationCurve _easeIn = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _easeOut = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Modes")]
        [SerializeField] private EnterMode _enter = EnterMode.PopIn;
        [SerializeField] private ExitMode _exit = ExitMode.PopOut;

        [Header("Offscreen math")]
        [Tooltip("Насколько уезжать за предел экрана.")]
        [SerializeField] private float _offscreenMargin = 32f;

        private RectTransform _rt;
        private RectTransform _rootRect;
        private CanvasGroup _cg;
        private Vector2 _onScreenAnchored;
        private bool _isInitialized = false; // Флаг, чтобы инициализировать только 1 раз

        private void EnsureInitialized()
        {
            if (_isInitialized) return;

            _rt = GetComponent<RectTransform>();
            _cg = GetComponent<CanvasGroup>();
            if (!_cg) _cg = gameObject.AddComponent<CanvasGroup>();

            // Запоминаем исходную позицию ТОЛЬКО ЗДЕСЬ
            // Это гарантирует, что мы запомним позицию из редактора,
            // а не позицию "за экраном" после предыдущего Hide.
            if (_rt != null)
            {
                _onScreenAnchored = _rt.anchoredPosition;
            }

            var canvas = GetRootCanvas(_rt);
            if (!canvas)
                // Можно оставить Warning, чтобы не спамило Error, если тестируешь префаб без канваса
                Debug.LogWarning("[UIShowHide] Root Canvas не найден (возможно, объект не под Canvas).");
            else
                _rootRect = canvas.GetComponent<RectTransform>();

            _isInitialized = true;
        }

        void Awake()
        {
            EnsureInitialized();
        }

        void OnEnable()
        {
            if (_playOnEnable)
            {
                PrepareEnterState();
                PlayEnter();
            }
        }

        // ========== Публичный API ==========
        public void Show()
        {
            EnsureInitialized(); // На случай если Awake еще не успел сработать
            gameObject.SetActive(true);
            PrepareEnterState();
            PlayEnter();
        }
        public void Show(Action endAnimation)
        {
            EnsureInitialized();
            gameObject.SetActive(true);
            PrepareEnterState();
            PlayEnter(endAnimation);
        }

        public void Hide()
        {
            PlayExit(() =>
            {
                if (_deactivateAfterHide) gameObject.SetActive(false);
            });
        }

        public void Hide(Action endAnimation)
        {
            PlayExit(() =>
            {
                if (_deactivateAfterHide) gameObject.SetActive(false);
                endAnimation?.Invoke();
            });
        }

        // ========== Логика подготовки ==========
        void PrepareEnterState()
        {
            EnsureInitialized();

            // ВАЖНО: Убрал отсюда перезапись _onScreenAnchored = _rt.anchoredPosition;
            // Иначе при повторном открытии мы запомним "спрятанную" позицию как целевую.

            ForceLayoutNow();

            if (_stopExistingTweens) this.StopAllTweens();
            if (_lockInteractionDuringTween) _cg?.DisableUITween();

            switch (_enter)
            {
                case EnterMode.FromLeft:
                case EnterMode.FromRight:
                case EnterMode.FromTop:
                case EnterMode.FromBottom:
                case EnterMode.FromLeftAndFade:
                case EnterMode.FromRightAndFade:
                case EnterMode.FromTopAndFade:
                case EnterMode.FromBottomAndFade:
                    {
                        // Ставим в позицию ЗА экраном
                        _rt.anchoredPosition = GetOffscreenAnchoredPos(DirFromEnter(_enter));
                        _rt.localScale = Vector3.one;
                        _cg.alpha = _enter.ToString().EndsWith("AndFade") ? 0f : 1f;
                        break;
                    }
                case EnterMode.ScaleIn:
                    // Ставим в позицию НА экране (она сохранена в EnsureInitialized)
                    _rt.anchoredPosition = _onScreenAnchored;
                    _rt.localScale = Vector3.one * 0.1f;
                    _cg.alpha = 1f;
                    break;
                case EnterMode.FadeIn:
                    _rt.anchoredPosition = _onScreenAnchored;
                    _rt.localScale = Vector3.one;
                    _cg.alpha = 0f;
                    break;
                case EnterMode.PopIn:
                    _rt.anchoredPosition = _onScreenAnchored;
                    _rt.localScale = Vector3.one * 0.85f;
                    _cg.alpha = 0f;
                    break;
            }
        }

        // ========== Запуск входа ==========
        async void PlayEnter(Action endAnimation = null)
        {
            if (_delayIn > 0f)
                await PGTween.Delay(_delayIn, _useIgnoreTimeScale);

            // Позиция
            if (IsDirectionalEnter(_enter))
            {
                PGTween.OnValueTween(
                    _rt.anchoredPosition, _onScreenAnchored, _durationIn,
                    _useIgnoreTimeScale,
                    v => _rt.anchoredPosition = v,
                    _easeIn,
                    () =>
                    {
                        if (_lockInteractionDuringTween) _cg?.EnableUITween();
                        endAnimation?.Invoke();
                    }
                );

                if (_enter.ToString().EndsWith("AndFade"))
                {
                    _cg?.OnAlphaTween(1f, _durationIn, _useIgnoreTimeScale, _easeIn);
                }
            }
            else if (_enter == EnterMode.ScaleIn)
            {
                _rt.transform.OnTransformScaleTween(Vector3.one, _durationIn, _useIgnoreTimeScale, _easeIn,
                    () =>
                    {
                        if (_lockInteractionDuringTween) _cg?.EnableUITween();
                        endAnimation?.Invoke();
                    });
            }
            else if (_enter == EnterMode.FadeIn)
            {
                _cg?.OnAlphaTween(1f, _durationIn, _useIgnoreTimeScale, _easeIn,
                    () =>
                    {
                        if (_lockInteractionDuringTween) _cg?.EnableUITween();
                        endAnimation?.Invoke();
                    });
            }
            else if (_enter == EnterMode.PopIn)
            {
                _rt.transform.OnTransformScaleTween(Vector3.one, _durationIn, _useIgnoreTimeScale, _easeIn, null);
                _cg?.OnAlphaTween(1f, _durationIn * 0.9f, _useIgnoreTimeScale, _easeIn,
                    () =>
                    {
                        if (_lockInteractionDuringTween) _cg?.EnableUITween();
                        endAnimation?.Invoke();
                    });
            }
        }

        // ========== Запуск выхода ==========
        async void PlayExit(Action onComplete)
        {
            EnsureInitialized(); // На случай вызова Hide без предварительного Show

            if (_stopExistingTweens) this.StopAllTweens();
            if (_lockInteractionDuringTween) _cg?.DisableUITween();

            if (_delayOut > 0f)
                await PGTween.Delay(_delayOut, _useIgnoreTimeScale);

            if (IsDirectionalExit(_exit))
            {
                var to = GetOffscreenAnchoredPos(DirFromExit(_exit));
                PGTween.OnValueTween(
                    _rt.anchoredPosition, to, _durationOut,
                    _useIgnoreTimeScale,
                    v => _rt.anchoredPosition = v,
                    _easeOut,
                    () =>
                    {
                        if (_lockInteractionDuringTween) _cg?.EnableUITween();
                        onComplete?.Invoke();
                    }
                );

                if (_exit.ToString().EndsWith("AndFade"))
                {
                    _cg?.OnAlphaTween(0f, _durationOut, _useIgnoreTimeScale, _easeOut);
                }
            }
            else if (_exit == ExitMode.ScaleOut)
            {
                _rt.transform.OnTransformScaleTween(Vector3.one * 0.1f, _durationOut, _useIgnoreTimeScale, _easeOut,
                    () =>
                    {
                        if (_lockInteractionDuringTween) _cg?.EnableUITween();
                        onComplete?.Invoke();
                    });
            }
            else if (_exit == ExitMode.FadeOut)
            {
                _cg?.OnAlphaTween(0f, _durationOut, _useIgnoreTimeScale, _easeOut,
                    () =>
                    {
                        if (_lockInteractionDuringTween) _cg?.EnableUITween();
                        onComplete?.Invoke();
                    });
            }
            else if (_exit == ExitMode.PopOut)
            {
                _rt.transform.OnTransformScaleTween(Vector3.one * 0.85f, _durationOut, _useIgnoreTimeScale, _easeOut, onComplete);
                _cg?.OnAlphaTween(0f, _durationOut * 0.9f, _useIgnoreTimeScale, _easeOut);
            }
        }

        // ========== Вычисление позиций ==========
        enum Dir { Left, Right, Top, Bottom }

        bool IsDirectionalEnter(EnterMode m) =>
            m == EnterMode.FromLeft || m == EnterMode.FromRight ||
            m == EnterMode.FromTop || m == EnterMode.FromBottom ||
            m == EnterMode.FromLeftAndFade || m == EnterMode.FromRightAndFade ||
            m == EnterMode.FromTopAndFade || m == EnterMode.FromBottomAndFade;

        bool IsDirectionalExit(ExitMode m) =>
            m == ExitMode.ToLeft || m == ExitMode.ToRight ||
            m == ExitMode.ToTop || m == ExitMode.ToBottom ||
            m == ExitMode.ToLeftAndFade || m == ExitMode.ToRightAndFade ||
            m == ExitMode.ToTopAndFade || m == ExitMode.ToBottomAndFade;

        Dir DirFromEnter(EnterMode m)
        {
            switch (m)
            {
                case EnterMode.FromLeft:
                case EnterMode.FromLeftAndFade: return Dir.Left;
                case EnterMode.FromRight:
                case EnterMode.FromRightAndFade: return Dir.Right;
                case EnterMode.FromTop:
                case EnterMode.FromTopAndFade: return Dir.Top;
                case EnterMode.FromBottom:
                case EnterMode.FromBottomAndFade: return Dir.Bottom;
            }
            return Dir.Left;
        }
        Dir DirFromExit(ExitMode m)
        {
            switch (m)
            {
                case ExitMode.ToLeft:
                case ExitMode.ToLeftAndFade: return Dir.Left;
                case ExitMode.ToRight:
                case ExitMode.ToRightAndFade: return Dir.Right;
                case ExitMode.ToTop:
                case ExitMode.ToTopAndFade: return Dir.Top;
                case ExitMode.ToBottom:
                case ExitMode.ToBottomAndFade: return Dir.Bottom;
            }
            return Dir.Right;
        }

        Vector2 GetOffscreenAnchoredPos(Dir dir)
        {
            if (!_rootRect) return _rt.anchoredPosition;

            // Используем _onScreenAnchored вместо _rt.anchoredPosition как базу,
            // чтобы дельта считалась всегда от "центра", а не от текущего положения
            // (которое может быть уже смещено).
            var currentPos = _onScreenAnchored;

            var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_rootRect, _rt);
            // Корректируем bounds на разницу между реальной позицией и сохраненной "базой",
            // если вдруг UI смещен. Но для стабильности лучше считать от базы.

            float rrMinX = _rootRect.rect.xMin;
            float rrMaxX = _rootRect.rect.xMax;
            float rrMinY = _rootRect.rect.yMin;
            float rrMaxY = _rootRect.rect.yMax;

            float dx = 0f, dy = 0f;
            switch (dir)
            {
                // Тут bounds могут немного врать, если объект уже уехал, 
                // но для простых меню это допустимая погрешность.
                case Dir.Left: dx = (rrMinX - _offscreenMargin) - bounds.max.x; break;
                case Dir.Right: dx = (rrMaxX + _offscreenMargin) - bounds.min.x; break;
                case Dir.Top: dy = (rrMaxY + _offscreenMargin) - bounds.min.y; break;
                case Dir.Bottom: dy = (rrMinY - _offscreenMargin) - bounds.max.y; break;
            }

            var parent = _rt.parent as RectTransform;
            if (!parent) return currentPos + new Vector2(dx, dy);

            Vector3 deltaLocal3 = parent.InverseTransformVector(_rootRect.TransformVector(new Vector3(dx, dy, 0f)));
            Vector2 deltaLocal = new Vector2(deltaLocal3.x, deltaLocal3.y);

            return currentPos + deltaLocal;
        }


        // ========== Вспомогательные ==========
        static Canvas GetRootCanvas(Transform t)
        {
            Transform cur = t;
            while (cur)
            {
                var c = cur.GetComponent<Canvas>();
                if (c && c.isRootCanvas) return c;
                cur = cur.parent;
            }
            return null;
        }

        void ForceLayoutNow()
        {
            if (_rt == null) return;

            var parent = _rt.parent as RectTransform;
            if (parent) LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rt);
        }
    }
}