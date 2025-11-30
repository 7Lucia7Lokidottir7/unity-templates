using UnityEngine;

namespace PG.HealthSystem
{
    public class HealthHeart : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _hitTrigger = "hit";
        [SerializeField] private string _healTrigger = "heal";
        public void Damage() => _animator.SetTrigger(_hitTrigger);
        public void Heal() => _animator.SetTrigger(_healTrigger);
    }
}
