// QuestListItemUI.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.QuestSystem
{
    [RequireComponent(typeof(LayoutElement))]
    public class QuestListItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descText;
        [SerializeField] private Image _completedIcon;      // иконка‑галочка или просто актив/деактив объекта
        [SerializeField] private Color _currentColor = Color.cyan;
        [SerializeField] private Color _defaultColor = Color.white;
        private FontStyles _nameBaseFontStyle;
        private void Awake()
        {
            _nameBaseFontStyle = _nameText.fontStyle;
        }
        /// <summary>
        /// Настроить строку списка
        /// </summary>
        public void Setup(QuestData data, bool isCurrent)
        {
            _nameText.text = data.name;
            _descText.text = data.description;
            _descText.gameObject.SetActive(isCurrent);       // если хочется показывать описание только для текущего
            _completedIcon.gameObject.SetActive(data.isCompleted);
            _nameText.fontStyle = data.isCompleted ? FontStyles.Strikethrough : _nameBaseFontStyle;

            var color = isCurrent ? _currentColor : _defaultColor;
            _nameText.color = color;
        }
    }
}
