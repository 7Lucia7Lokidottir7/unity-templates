// PauseMenuQuestListUI.cs
using System.Collections.Generic;
using UnityEngine;
using PG.QuestSystem;

namespace PG.QuestSystem
{
    public class PauseMenuQuestListUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private QuestSystem _questSystem;
        [SerializeField] private Transform _contentParent;      // ���� ��������� ������ (������ Content � ScrollView)
        [SerializeField] private QuestListItemUI _itemPrefab;   // ������ � ����������� QuestListItemUI

        private List<QuestListItemUI> _spawnedItems = new List<QuestListItemUI>();
        private QuestLine _currentLine;

        private void OnEnable()
        {
            // ������������� �� ����� �����
            _questSystem.onLineStarted.AddListener(OnLineStarted);
            // ���� ����� ��� �������� �� ����� � ��������� �����
            if (_questSystem.currentLine != null)
                OnLineStarted(_questSystem.currentLine);
        }

        private void OnDisable()
        {
            _questSystem.onLineStarted.RemoveListener(OnLineStarted);
            if (_currentLine != null)
                _currentLine.onQuestChanged.RemoveListener(OnQuestChanged);
        }

        private void OnLineStarted(QuestLine line)
        {
            // ������������ �� ������
            if (_currentLine != null)
                _currentLine.onQuestChanged.RemoveListener(OnQuestChanged);

            _currentLine = line;
            // ������������� �� ������� ������ �����
            _currentLine.onQuestChanged.AddListener(OnQuestChanged);
            // � ����� �� ��������� ������
            RefreshList();
        }

        private void OnQuestChanged(QuestData value)
        {
            // ����� ��������� ������� ��� ������ ���������� � ������ ���������� ��
            RefreshList();
        }

        private void RefreshList()
        {
            // ������� ���������� ���������
            foreach (var item in _spawnedItems)
                Destroy(item.gameObject);
            _spawnedItems.Clear();

            if (_currentLine == null) return;

            var quests = _currentLine.quests;
            for (int i = 0; i < quests.Count; i++)
            {
                var data = quests[i];
                var item = Instantiate(_itemPrefab, _contentParent);
                item.Setup(data, i == _currentLine.currentIndex);
                _spawnedItems.Add(item);
            }
        }
    }
}
