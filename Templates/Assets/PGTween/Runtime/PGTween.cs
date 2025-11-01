using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.Tween
{
    public static class PGTween
    {
        private static bool isStopTween;
        private static CancellationTokenSource cancellationTokenSource;
        public static async void StopAllTweens(this MonoBehaviour monoBehaviour)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }

            isStopTween = true;
            await Task.Delay(75);
            isStopTween = false;
        }
        public static async Task Delay(float time, bool useIgnoreTimeScale = false, System.Action ended = null)
        {
            if (useIgnoreTimeScale)
            {
                for (float i = 0; i < time; i += Time.unscaledDeltaTime)
                {
                    if (isStopTween)
                    {
                        return;
                    }
                    await Task.Yield();
                }
            }
            else
            {
                for (float i = 0; i < time; i += Time.deltaTime)
                {
                    if (isStopTween)
                    {
                        return;
                    }
                    await Task.Yield();
                }
            }
            ended?.Invoke();
        }
        public static async Task DelayTask(float time, System.Action ended = null)
        {
            await Task.Delay((int)(time * 1000));
            ended?.Invoke();
        }

        public static async void OnMaterialValueTween(this Material material, string parameter, float to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            float elapsedTime = 0f;
            float from = material.GetFloat(parameter);

            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (!material)
                {
                    return;
                }
                material.SetFloat(parameter, Mathf.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time)));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            material.SetFloat(parameter, to);
            ended?.Invoke();
        }
        public static async void OnMaterialValueTween(this Material material, string parameter, Color to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            float elapsedTime = 0f;
            Color from = material.GetColor(parameter);
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (!material)
                {
                    return;
                }
                material.SetColor(parameter, Color.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time)));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            material.SetColor(parameter, to);
            ended?.Invoke();
        }

        public static async void OnAlphaTween(this CanvasGroup canvasGroup, float to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            float elapsedTime = 0f;
            float from = canvasGroup.alpha;
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (canvasGroup == null)
                {
                    return;
                }
                canvasGroup.alpha = Mathf.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            canvasGroup.alpha = to;
            ended?.Invoke();
        }
        public static async void OnAlphaTween(this Image image, float to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            Color from = image.color;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (!image)
                {
                    return;
                }
                image.color = Color.LerpUnclamped(from, new Color(image.color.r, image.color.r, image.color.r, to), animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            image.color = new Color(image.color.r, image.color.r, image.color.r, to);
            ended?.Invoke();
        }
        public static async void OnAlphaTween(this TMP_Text tmpText, float to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            float from = tmpText.alpha;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (!tmpText)
                {
                    return;
                }
                tmpText.alpha = Mathf.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            tmpText.alpha = to;
            ended?.Invoke();
        }
        public static async void OnAlphaTween(this Material material, float to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            float elapsedTime = 0f;
            Color from = material.color;
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (!material)
                {
                    return;
                }
                material.color = Color.LerpUnclamped(from, new Color(material.color.r, material.color.r, material.color.r, to), animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            material.color = new Color(material.color.r, material.color.r, material.color.r, to);
            ended?.Invoke();
        }

        public static async void OnColorTween(this Image image, Color to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            float elapsedTime = 0f;
            Color from = image.color; // Запоминаем начальный цвет

            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (!image)
                {
                    return;
                }

                float t = elapsedTime / time; // Вычисляем прогресс от 0 до 1

                // Интерполируем цвет между начальным и целевым цветом
                image.color = Color.LerpUnclamped(from, to, animationCurve.Evaluate(t));

                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            image.color = to;
            ended?.Invoke();
        }
        public static async void OnColorTween(this Material material, Color to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            float elapsedTime = 0f;
            Color from = material.color; // Запоминаем начальный цвет

            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            while (elapsedTime < time)
            {
                if (!material)
                {
                    return;
                }
                if (isStopTween)
                {
                    return;
                }
                float t = elapsedTime / time; // Вычисляем прогресс от 0 до 1

                // Интерполируем цвет между начальным и целевым цветом
                material.color = Color.LerpUnclamped(from, to, animationCurve.Evaluate(t));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            material.color = to;
        }

        public static async void OnTransformTween(this Transform transform, Vector3 to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            Vector3 from = transform.position;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (transform == null)
                {
                    return;
                }
                transform.position = Vector3.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            transform.position = to;
            ended?.Invoke();
        }

        public static async void OnTransformLocalTween(this Transform transform, Vector3 to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            Vector3 from = transform.localPosition;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (transform == null)
                {
                    return;
                }
                transform.localPosition = Vector3.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            transform.localPosition = to;
            ended?.Invoke();
        }

        public static async void OnTransformLocalEulerAnglesTween(this Transform transform, Vector3 to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            Vector3 from = transform.localEulerAngles;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (transform == null)
                {
                    return;
                }
                transform.localEulerAngles = Vector3.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            transform.localEulerAngles = to;
            ended?.Invoke();
        }

        public static async void OnTransformEulerAnglesTween(this Transform transform, Vector3 to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            Vector3 from = transform.eulerAngles;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (transform == null)
                {
                    return;
                }
                transform.eulerAngles = Vector3.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            transform.eulerAngles = to;
            ended?.Invoke();
        }

        public static async void OnTransformScaleTween(this Transform transform, Vector3 to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            Vector3 from = transform.localScale;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (transform == null)
                {
                    return;
                }
                transform.localScale = Vector3.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            transform.localScale = to;
            ended?.Invoke();
        }

        public static async void OnTransformRotationTween(this Transform transform, Quaternion to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            Quaternion from = transform.rotation;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (transform == null)
                {
                    return;
                }
                transform.rotation = Quaternion.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            transform.rotation = to;
            ended?.Invoke();
        }

        public static async void OnTransformLocalRotationTween(this Transform transform, Quaternion to, float time, bool useIgnoreTimeScale = false, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            Quaternion from = transform.localRotation;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                if (transform == null)
                {
                    return;
                }
                transform.localRotation = Quaternion.LerpUnclamped(from, to, animationCurve.Evaluate(elapsedTime / time));
                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                await Task.Yield();
            }
            transform.localRotation = to;
            ended?.Invoke();
        }

        public static async void OnValueTween(float from, float to, float time, bool useIgnoreTimeScale = false, System.Action<float> onUpdate = default, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                float t = elapsedTime / time;
                float value = Mathf.LerpUnclamped(from, to, animationCurve.Evaluate(t));
                onUpdate?.Invoke(value);

                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }

                await Task.Yield();

                // Проверяем, существует ли объект onUpdate


            }

            // Убедимся, что последнее значение соответствует точному значению "to"
            onUpdate.Invoke(to);
            ended?.Invoke();
        }
        public static async void OnValueTween(Quaternion from, Quaternion to, float time, bool useIgnoreTimeScale = false, System.Action<Quaternion> onUpdate = default, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                float t = elapsedTime / time;
                Quaternion value = Quaternion.LerpUnclamped(from, to, animationCurve.Evaluate(t));
                onUpdate?.Invoke(value);

                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }

                await Task.Yield();

                // Проверяем, существует ли объект onUpdate


            }

            // Убедимся, что последнее значение соответствует точному значению "to"
            onUpdate.Invoke(to);
            ended?.Invoke();
        }

        public static async void OnValueTween(Vector3 from, Vector3 to, float time, bool useIgnoreTimeScale = false, System.Action<Vector3> onUpdate = default, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                float t = elapsedTime / time;
                Vector3 value = Vector3.LerpUnclamped(from, to, animationCurve.Evaluate(t));
                onUpdate?.Invoke(value);

                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }

                await Task.Yield();

            }

            // Убедимся, что последнее значение соответствует точному значению "to"
            onUpdate.Invoke(to);
            ended?.Invoke();
        }

        public static async void OnValueTween(Vector2 from, Vector2 to, float time, bool useIgnoreTimeScale = false, System.Action<Vector2> onUpdate = default, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                float t = elapsedTime / time;
                Vector2 value = Vector2.LerpUnclamped(from, to, animationCurve.Evaluate(t));
                onUpdate?.Invoke(value);

                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }

                await Task.Yield();

                // Проверяем, существует ли объект onUpdate


            }

            // Убедимся, что последнее значение соответствует точному значению "to"
            onUpdate.Invoke(to);
            ended?.Invoke();
        }

        public static async void OnValueTween(Color from, Color to, float time, bool useIgnoreTimeScale = false, System.Action<Color> onUpdate = default, AnimationCurve animationCurve = null, System.Action ended = null)
        {
            if (isStopTween)
            {
                return;
            }
            if (animationCurve == null)
            {
                animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                if (isStopTween)
                {
                    return;
                }
                float t = elapsedTime / time;
                Color value = Color.LerpUnclamped(from, to, animationCurve.Evaluate(t));
                onUpdate?.Invoke(value);

                if (useIgnoreTimeScale)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }

                await Task.Yield();

                // Проверяем, существует ли объект onUpdate


            }

            // Убедимся, что последнее значение соответствует точному значению "to"
            onUpdate.Invoke(to);
            ended?.Invoke();
        }
        public static void EnableUITween(this CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        public static void DisableUITween(this CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
