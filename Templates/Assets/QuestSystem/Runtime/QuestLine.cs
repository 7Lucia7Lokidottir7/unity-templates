using UnityEngine;

namespace PG.QuestSystem
{
    [CreateAssetMenu(menuName = "PG/Quest System/Quest Line")]
    public class QuestLine : ScriptableObject
    {
        public string _nameQuest;
        [TextArea] public string _descriptionQuest;

        public string[] quests;
        public int currentQuest;
        public string quest => (currentQuest >= 0 && currentQuest < quests.Length) ?  quests[currentQuest] : "";
        public bool isActive;
        public bool isCompleted { get; private set; }
        public bool NextQuest()
        {
            currentQuest++;
            isCompleted = currentQuest >= quests.Length;
            isActive = false;
            return isCompleted;
        }
        public void StartQuest()
        {
            isActive = true;
            currentQuest = 0;
            isCompleted = false;
        }
    }
}
