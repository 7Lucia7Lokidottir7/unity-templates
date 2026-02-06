using System.Collections.Generic;
using UnityEngine;

namespace PG.ModuleManagement
{
    public interface IModuleManager
    {
        public event System.Action<bool> enabledEvent;
        public event System.Action<GameObject> updateEvent;
        public bool enabled { get; set; }
        public List<IModule> modules { get; set; }
    }
}