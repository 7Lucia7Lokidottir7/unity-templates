// QuestSystem.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PG.QuestSystem
{
    public class QuestSystem : MonoBehaviour
    {
        [SerializeField] private List<QuestLine> _questLines = new List<QuestLine>();
        [SerializeField] private int _startLineIndex = 0;

        public UnityEvent<QuestLine> onLineStarted;
        public UnityEvent<QuestLine> onLineCompleted;

        private QuestLine _currentLine;
        public QuestLine currentLine => _currentLine;

        private void Awake()
        {
            if (_questLines.Count == 0)
                Debug.LogWarning("Нет ни одной QuestLine в QuestSystem!");

            // Подписываемся на каждую линию
            foreach (var line in _questLines)
            {
                line.onLineCompleted.AddListener(HandleLineCompleted);
            }
        }

        private void Start()
        {
            StartLine(_startLineIndex);
        }

        public void StartLine(int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= _questLines.Count)
            {
                Debug.LogError($"Неверный индекс линии: {lineIndex}");
                return;
            }

            _currentLine = _questLines[lineIndex];
            _currentLine.StartLine();
            onLineStarted?.Invoke(_currentLine);
        }

        private void HandleLineCompleted(QuestLine completedLine)
        {
            onLineCompleted?.Invoke(completedLine);
            // Здесь можно автоматически переключиться на следующую, если нужно
        }
    }
}
