using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.MenuManagement
{
    [AddComponentMenu("UI/Button With Text")]
    public class ButtonWithText : Button
    {
        [Header("Text Reference")]
        public TMP_Text textObject;

        [Header("Text Colors")]
        public ColorBlock textColors = ColorBlock.defaultColorBlock;

        [ContextMenu("Get Cache Text")]
        void GetCacheText()
        {
            textObject = transform.GetComponentInChildren<TMP_Text>();
    #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
    #endif
        }

        protected override void Awake()
        {
            base.Awake();

            if (textObject == null)
            {
                textObject = GetComponentInChildren<TMP_Text>();
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (textObject == null) return;

            Color targetColor;

            switch (state)
            {
                case SelectionState.Normal:
                    targetColor = textColors.normalColor;
                    break;
                case SelectionState.Highlighted:
                    targetColor = textColors.highlightedColor;
                    break;
                case SelectionState.Pressed:
                    targetColor = textColors.pressedColor;
                    break;
                case SelectionState.Selected:
                    targetColor = textColors.highlightedColor;
                    break;
                case SelectionState.Disabled:
                    targetColor = textColors.disabledColor;
                    break;
                default:
                    targetColor = textObject.color;
                    break;
            }

            textObject.color = targetColor;
        }
    }

}

