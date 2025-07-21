// QuestUI.cs
using UnityEngine;
using TMPro;

namespace PG.QuestSystem
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private QuestSystem _questSystem;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descText;

        private void OnEnable()
        {
            _questSystem.onLineStarted.AddListener(OnLineStarted);
            _questSystem.currentLine.onQuestChanged.AddListener(OnQuestChanged);
        }

        private void OnDisable()
        {
            _questSystem.onLineStarted.RemoveListener(OnLineStarted);
            if (_questSystem.currentLine != null)
                _questSystem.currentLine.onQuestChanged.RemoveListener(OnQuestChanged);
        }

        private void OnLineStarted(QuestLine line)
        {
            // Отпишемся от старой и подпишемся на новую
            if (_questSystem.currentLine != null)
                _questSystem.currentLine.onQuestChanged.RemoveListener(OnQuestChanged);

            line.onQuestChanged.AddListener(OnQuestChanged);
            UpdateUI(line.currentQuest);
        }

        private void OnQuestChanged(QuestData quest)
        {
            UpdateUI(quest);
        }

        private void UpdateUI(QuestData quest)
        {
            _nameText.text = quest.name;
            if (_descText != null)
                _descText.text = quest.description;
        }
    }
}
