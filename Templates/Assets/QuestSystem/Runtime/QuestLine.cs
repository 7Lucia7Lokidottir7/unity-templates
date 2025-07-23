using UnityEngine;

namespace PG.QuestSystem
{
    [CreateAssetMenu(menuName = "PG/Quest System/")]
    public class QuestLine : ScriptableObject
    {
        public string _nameQuest;
        [TextArea] public string _descriptionQuest;

        public string[] quests;
        public int currentQuest;
        public string quest => (currentQuest >= 0 && currentQuest < quests.Length) ?  quests[currentQuest] : "";
        public bool isActive;
        public bool isCompleted;
    }
}
