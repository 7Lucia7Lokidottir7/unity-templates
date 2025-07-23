using UnityEngine;

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
    }
}
