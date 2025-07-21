// QuestData.cs
using System;
using UnityEngine;

namespace PG.QuestSystem
{
    [Serializable]
    public class QuestData
    {
        [SerializeField] private string _name;
        [SerializeField][TextArea] private string _description;

        public string name => _name;
        public string description => _description;

        public event Action<QuestData> onCompleted;

        private bool _isCompleted;
        public bool isCompleted
        {
            get => _isCompleted;
            set
            {
                if (!_isCompleted && value)
                {
                    _isCompleted = true;
                    onCompleted?.Invoke(this);
                }
            }
        }
    }
}
