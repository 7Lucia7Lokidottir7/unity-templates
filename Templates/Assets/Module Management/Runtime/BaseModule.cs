using UnityEngine;

namespace PG.ModuleManagement
{
    [System.Serializable]
    public abstract class BaseModule : IModule
    {
        // Ссылка на менеджера, если вдруг понадобится получить от него данные
        protected IModuleManager Manager;

        public virtual void Initialize(IModuleManager manager)
        {
            Manager = manager;

            // --- МАГИЯ ПОДПИСКИ ---
            // Подписываемся на обновление
            Manager.updateEvent += OnUpdate;

            // Подписываемся на включение/выключение
            Manager.enabledEvent += OnStateChanged;
        }

        // Обработчик переключения состояния (конвертирует bool в вызов методов)
        private void OnStateChanged(bool isEnabled)
        {
            if (isEnabled) OnEnable();
            else OnDisable();
        }

        // Абстрактный метод, который вы обязаны реализовать (логика)
        public abstract void OnUpdate(GameObject owner);

        // Виртуальные методы (можно не переопределять, если не нужны)
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
    }
}