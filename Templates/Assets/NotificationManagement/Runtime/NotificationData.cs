using UnityEngine;

namespace PG.NotificationManagement
{
    [System.Serializable]
    public struct NotificationData
    {
        public string name;
        public string description;
        public Sprite icon;

        public NotificationData(string name, string description, Sprite icon)
        {
            this.name = name;
            this.description = description;
            this.icon = icon;
        }
    }
}