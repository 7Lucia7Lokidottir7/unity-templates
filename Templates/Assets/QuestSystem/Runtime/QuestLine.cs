// QuestLine.cs
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PG.QuestSystem
{
    [CreateAssetMenu(menuName = "PG/Quest System/Quest Line")]
    public class QuestLine : ScriptableObject
    {
        [SerializeField] private QuestData[] _quests;
        public IReadOnlyList<QuestData> quests => _quests;

        [SerializeField, Min(0)] private int _startIndex = 0;
        public int currentIndex { get; private set; }

        public QuestData currentQuest =>
            (_quests != null && currentIndex >= 0 && currentIndex < _quests.Length)
            ? _quests[currentIndex]
            : null;

        public UnityEvent<QuestData> onQuestChanged;
        public UnityEvent<QuestLine> onLineCompleted;

        private void OnEnable()
        {
            // Подписываемся на событие каждого квеста
            foreach (var q in _quests)
            {
                q.onCompleted += HandleQuestCompleted;
            }
            ResetLine();
        }

        private void OnDisable()
        {
            foreach (var q in _quests)
            {
                q.onCompleted -= HandleQuestCompleted;
            }
        }

        public void StartLine()
        {
            ResetLine();
            NotifyQuestChanged();
        }

        private void ResetLine()
        {
            currentIndex = Mathf.Clamp(_startIndex, 0, _quests.Length - 1);
            foreach (var q in _quests)
                q.isCompleted = false;
        }

        private void HandleQuestCompleted(QuestData completedQuest)
        {
            // Если это текущий квест — переходим дальше
            if (completedQuest == currentQuest)
            {
                AdvanceQuest();
            }
        }

        private void AdvanceQuest()
        {
            currentIndex++;
            if (currentIndex < _quests.Length)
            {
                NotifyQuestChanged();
            }
            else
            {
                onLineCompleted?.Invoke(this);
            }
        }

        private void NotifyQuestChanged()
        {
            onQuestChanged?.Invoke(currentQuest);
        }
    }
}
