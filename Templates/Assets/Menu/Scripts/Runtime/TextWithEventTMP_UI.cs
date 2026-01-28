using TMPro;
using UnityEngine;

namespace PG.MenuManagement
{
    [AddComponentMenu("UI/Text With Event TMP_UI")]
    public class TextWithEventTMP_UI : TextMeshProUGUI
    {
        public System.Action<string> onTextChanged;
        public override string text
        {
            get => base.text;
            set
            {
                if (base.text != value)
                {
                    base.text = value;
                    onTextChanged?.Invoke(value);
                }
            }
        }
    }

}

