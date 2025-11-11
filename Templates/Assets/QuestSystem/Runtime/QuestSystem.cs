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
        private void Awake()
        {
            for (int i = 0; i < questLines.Length; i++)
            {
                questLines[i] = Instantiate(questLines[i]);
            }

            _watchButton.onClick.AddListener(WatchQuest);
            for (int i = 0; i < _cellContainer.childCount; i++)
            {
                Button button = _cellContainer.GetChild(i).GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    DisplayQuestText();
                    _selectedQuest = i;
                });
            }
        }
        public void ActivateQuest(QuestLine questLine)
        {
            questLine.StartQuest();
            questStarted?.Invoke(questLine);
        }
        public void ActivateQuest(int questLineID)
        {
            questLines[questLineID].StartQuest();
            questStarted?.Invoke(questLines[questLineID]);
        }
        public void NextQuest(QuestLine questLine)
        {
            if (questLine.NextQuest())
            {
                questCompleted?.Invoke(questLine);
            }
        }
        public void NextQuest(int questLineID)
        {
            if (questLines[questLineID].NextQuest())
            {
                questCompleted?.Invoke(questLines[questLineID]);
            }
        }
        public void NextTargetQuest()
        {
            if (targetQuestLine.NextQuest())
            {
                questCompleted?.Invoke(targetQuestLine);
            }
        }
        public void DisplayCells()
        {
            if (_selectedQuest < 0 && _selectedQuest >= questLines.Length)
            {
                _selectedQuest = 0;
            }
            else
            {
                if (!questLines[_selectedQuest].isActive)
                {
                    _selectedQuest = 0;
                }
            }
            for (int i = 0; i < _cellContainer.childCount; i++)
            {
                if (i < questLines.Length)
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
        }
        public void WatchQuest()
        {
            targetQuestLineID = _selectedQuest;
            questChanged.Invoke(targetQuestLine);
        }
        void DisplayQuestText()
        {
            QuestLine selectedQuestLine = questLines[_selectedQuest];
            _titleObject.text = selectedQuestLine._nameQuest;
            _descriptionObject.text = selectedQuestLine._descriptionQuest;
            _statusTextObject.gameObject.SetActive(selectedQuestLine == targetQuestLine);
            displayedQuest?.Invoke(selectedQuestLine);
        }
    }
}
