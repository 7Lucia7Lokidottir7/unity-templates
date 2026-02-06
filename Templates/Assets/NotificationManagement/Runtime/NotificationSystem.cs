using System.Collections.Generic;
using UnityEngine;

namespace PG.NotificationManagement
{
    public class NotificationSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform _cellContainer;
        [SerializeField] private NotificationView _prefab;
        [SerializeField] private float _duration = 2f;
        [SerializeField] private int _initialPoolSize = 5; // Предзагрузка объектов

        // Стек для хранения неактивных объектов (наш Пул)
        private Stack<NotificationView> _pool = new Stack<NotificationView>();

        public static NotificationSystem instance;

        void InitializeInstance()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(instance.gameObject);
        }
        private void Awake()
        {
            InitializeInstance();

            // Предварительное создание объектов, чтобы не лагало при первом уведомлении
            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateNewItemAndAddToPool();
            }
        }

        public void Add(string title, string description, Sprite icon)
        {
            NotificationData data = new NotificationData(title, description, icon);
            ShowNotification(data);
        }

        private void ShowNotification(NotificationData data)
        {
            NotificationView view;

            // 1. Берем из пула или создаем новый
            if (_pool.Count > 0)
            {
                view = _pool.Pop();
            }
            else
            {
                view = InstantiateView();
            }

            // 2. Активируем и помещаем в конец списка (чтобы порядок в LayoutGroup был верным)
            view.transform.SetAsLastSibling();
            view.gameObject.SetActive(true);

            // 3. Настраиваем и передаем метод возврата в пул
            view.Setup(data, _duration, ReturnToPool);
        }

        private void ReturnToPool(NotificationView view)
        {
            view.gameObject.SetActive(false); // Выключаем объект
            _pool.Push(view); // Возвращаем в стек
        }

        private NotificationView InstantiateView()
        {
            NotificationView newView = Instantiate(_prefab, _cellContainer);
            newView.gameObject.SetActive(false);
            return newView;
        }

        private void CreateNewItemAndAddToPool()
        {
            var view = InstantiateView();
            _pool.Push(view);
        }
    }
}