using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.BattleSystem
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _attackProperty;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _attackParameter = "Attack";
        [SerializeField] private float _turnDuration = 0.1f;
        [SerializeField] private float _checkEnemyRadius = 1f;
        [SerializeField] private LayerMask _targetLayers;
        private Coroutine _turnCoroutine;
        public Transform targetEnemy;
        private void OnEnable()
        {
            _attackProperty.action.performed += OnAttack;
        }
        private void OnDisable()
        {
            _attackProperty.action.performed -= OnAttack;
        }
        void Start()
        {
        
        }
        void OnAttack(InputAction.CallbackContext context)
        {
            _animator.SetTrigger(_attackParameter);
            if (_turnCoroutine != null)
            {
                StopCoroutine(_turnCoroutine);
            }
            _turnCoroutine = StartCoroutine(TurnToEnemy());
        }
        IEnumerator TurnToEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(_animator.transform.position, _checkEnemyRadius, _targetLayers);
            float maxDistance = Mathf.Infinity;
            for (int i = 0; i < enemies.Length; i++)
            {
                float distance = Vector3.Distance(_animator.transform.position, enemies[i].transform.position);
                if (distance < maxDistance)
                {
                    maxDistance = distance;
                    targetEnemy = enemies[i].transform;
                }
            }

            if (targetEnemy == null)
            {
                yield break;
            }

            for (float i = 0; i < _turnDuration; i+= Time.deltaTime)
            {
                Vector3 dir = (targetEnemy.position - _animator.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0 , dir.z));
                _animator.transform.rotation = Quaternion.Lerp(_animator.transform.rotation, lookRotation, i / _turnDuration);
                yield return null;
            }
        }
    }
}
