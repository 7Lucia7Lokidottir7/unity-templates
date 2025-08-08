using UnityEngine;

namespace PG.ShootSystem
{
    public class InputAnimationShooter : InputShooter
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _shootParameter = "Shoot";

        public override void OnShoot()
        {
            if (!isAuto)
            {
                _animator.SetBool(_shootParameter, true);
            }
            else
            {
                _animator.SetTrigger(_shootParameter);
            }
        }

        public override void OnShootEnd()
        {
            _animator.SetBool(_shootParameter, false);
        }
    }
}
