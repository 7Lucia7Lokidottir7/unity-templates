using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.MenuManagement
{
    [AddComponentMenu("UI/TMP_Dropdown With Text")]
    public class TMP_DropdownWithText : TMP_Dropdown
    {

        [Header("Text Colors")]
        public ColorBlock textColors = ColorBlock.defaultColorBlock;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (captionText == null) return;

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
                    targetColor = captionText.color;
                    break;
            }

            captionText.color = targetColor;
        }
    }

}

