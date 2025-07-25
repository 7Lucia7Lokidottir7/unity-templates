using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.QuestSystem
{
    public class QuestSystem : MonoBehaviour
    {
        public QuestLine[] questLines;
        public event System.Action<QuestLine> questStarted;
        public event System.Action<QuestLine> questChanged;
        public event System.Action<QuestLine> questCompleted;
        public int targetQuestLineID;
        public QuestLine targetQuestLine => questLines[targetQuestLineID];
        [SerializeField] private Transform _cellContainer;
        [SerializeField] private TMP_Text _titleObject;
        [SerializeField] private TMP_Text _descriptionObject;
        [SerializeField] private TMP_Text _statusTextObject;
        [SerializeField] private Button _watchButton;
        private int _selectedQuest;
        public event System.Action<QuestLine> displayedQuest;
        public void DisplayCells()
        {
            for (int i = 0; i < questLines.Length; i++)
            {
                QuestLine questLine = questLines[i];
                Transform cell = _cellContainer.GetChild(i);
                cell.gameObject.SetActive(questLine.isActive);

                TMP_Text nameQuestObject = cell.GetChild(0).GetComponent<TMP_Text>();
                nameQuestObject.text = questLine.name;

                GameObject statusObject = cell.GetChild(1).gameObject;
                statusObject.SetActive(i == targetQuestLineID);
            }
        }
        public void WatchQuest()
        {
            targetQuestLineID = _selectedQuest;
            questChanged.Invoke(targetQuestLine);
        }
        void DisplayQuestText()
        {
            _watchButton.onClick.RemoveListener(WatchQuest);
            QuestLine selectedQuestLine = questLines[_selectedQuest];
            _titleObject.text = selectedQuestLine._nameQuest;
            _descriptionObject.text = selectedQuestLine._descriptionQuest;
            _statusTextObject.gameObject.SetActive(selectedQuestLine == targetQuestLine);
            displayedQuest?.Invoke(selectedQuestLine);
            _watchButton.onClick.AddListener(WatchQuest);
        }
    }
}
