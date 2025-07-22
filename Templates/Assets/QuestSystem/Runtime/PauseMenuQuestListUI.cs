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
        [SerializeField] private Transform _contentParent;      // куда инстансим €чейки (обычно Content в ScrollView)
        [SerializeField] private QuestListItemUI _itemPrefab;   // префаб с компонентом QuestListItemUI

        private List<QuestListItemUI> _spawnedItems = new List<QuestListItemUI>();
        private QuestLine _currentLine;

        private void OnEnable()
        {
            // ѕодписываемс€ на смену линии
            _questSystem.onLineStarted.AddListener(OnLineStarted);
            // ≈сли лини€ уже запущена до паузы Ч обновл€ем сразу
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
            // отписываемс€ от старой
            if (_currentLine != null)
                _currentLine.onQuestChanged.RemoveListener(OnQuestChanged);

            _currentLine = line;
            // подписываемс€ на событи€ внутри линии
            _currentLine.onQuestChanged.AddListener(OnQuestChanged);
            // и сразу же заполн€ем список
            RefreshList();
        }

        private void OnQuestChanged(QuestData value)
        {
            // могут сменитьс€ текущий или статус выполнени€ Ч просто перерисуем всЄ
            RefreshList();
        }

        private void RefreshList()
        {
            // очистка предыдущих элементов
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
