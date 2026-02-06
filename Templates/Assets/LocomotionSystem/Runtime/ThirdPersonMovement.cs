using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.LocomotionSystem
{
    using ModuleManagement;
    public class ThirdPersonMovement : MonoBehaviour, IModuleManager
    {
        [SerializeField] private InputActionProperty _moveProperty;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _orientation;

        // Ваши настройки движения...
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _turnDuration = 4f;

        // СОБЫТИЯ
        public event System.Action<bool> enabledEvent;
        public event System.Action<GameObject> updateEvent;

        // СПИСОК МОДУЛЕЙ
        // Используем SubclassSelector для выбора модулей в инспекторе
        [SerializeReference, SubclassSelector]
        private List<IModule> _modules = new List<IModule>();

        // Реализация свойства интерфейса
        public List<IModule> modules { get => _modules; set => _modules = value; }

        private void Awake()
        {
            // Инициализация модулей (они здесь сами подпишутся на события)
            foreach (var module in _modules)
            {
                module.Initialize(this);
            }
        }

        private void OnEnable()
        {
            // Сообщаем модулям, что мы включились
            enabledEvent?.Invoke(true);
        }

        private void OnDisable()
        {
            // Сообщаем модулям, что мы выключились
            enabledEvent?.Invoke(false);
        }

        private void Update()
        {
            // 1. Сначала логика движения самого контроллера
            HandleMovement();

            // 2. Затем сообщаем всем модулям, что прошел кадр
            updateEvent?.Invoke(gameObject);
        }

        private void HandleMovement()
        {
            Vector2 input = _moveProperty.action.ReadValue<Vector2>().normalized;

            float moveAmount = Mathf.Abs(input.x) + Mathf.Abs(input.y);
            var moveInput = (new Vector3(input.x, 0, input.y)).normalized;
            Vector3 direction = Quaternion.Euler(0, _orientation.eulerAngles.y, 0) * moveInput;

            if (moveAmount > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                _characterController.transform.rotation = Quaternion.RotateTowards(
                    _characterController.transform.rotation,
                    lookRotation,
                    _turnDuration * Time.deltaTime
                );
            }

            _characterController.Move(direction * _moveSpeed * Time.deltaTime);
        }
    }
}