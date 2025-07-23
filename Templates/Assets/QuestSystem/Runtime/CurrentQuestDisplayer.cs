using TMPro;
using UnityEngine;

namespace PG.QuestSystem
{
    public class CurrentQuestDisplayer : MonoBehaviour
    {
        [SerializeField] private QuestSystem _questSystem;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _titleTargetQuestText;

        private void Awake()
        {
            if (_questSystem == null)
            {
                _questSystem = FindAnyObjectByType<QuestSystem>();
            }
        }
        void OnDisplay()
        {
            _titleText.text = _questSystem.targetQuestLine.name;
            _titleTargetQuestText.text = _questSystem.targetQuestLine.quest;
        }
    }
}
