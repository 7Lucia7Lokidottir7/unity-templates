using UnityEngine;

namespace PG.ModuleManagement
{
    public interface IModule
    {
        // Метод для связывания (подписки)
        void Initialize(IModuleManager manager);

        // Ваши методы логики
        void OnEnable();
        void OnDisable();
        void OnUpdate(GameObject owner);
    }
}