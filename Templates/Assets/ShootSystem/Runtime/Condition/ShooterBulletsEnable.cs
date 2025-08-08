using UnityEngine;

namespace PG.ShootSystem
{
    public class ShooterBulletsEnable : MonoBehaviour
    {
        [SerializeField] private InputShooter _inputShooter;
        [SerializeField, Min(0)] private int _bullets = 40;
        [Min(0)] public int bullets
        {
            get => _bullets;
            set
            {
                _bullets = value;
                _bullets = Mathf.Clamp(_bullets, 0, maxBullets);
                _inputShooter.isShootEnable = _bullets > 0;
            }
        }
        [Min(0)] public int maxBullets = 40;

        private void OnEnable()
        {
            _inputShooter.shooted += OnShoot;
        }
        private void OnDisable()
        {
            _inputShooter.shooted -= OnShoot;
        }
        void OnShoot(bool value)
        {
            if (value)
            {
                _bullets--;
            }
        }
    }
}
