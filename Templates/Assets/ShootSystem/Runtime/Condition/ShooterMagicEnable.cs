using UnityEngine;
using UnityEngine.UI;

namespace PG.ShootSystem
{
    public class ShooterMagicEnable : MonoBehaviour
    {
        [SerializeField] private InputShooter _inputShooter;
        [SerializeField] private Slider _energySlider;
        [SerializeField] private float _minEnergy = 12f;

        private void OnEnable()
        {
            _energySlider.onValueChanged.AddListener(OnShootEnable);
            _inputShooter.shooted += OnShoot;
        }
        private void OnDisable()
        {
            _energySlider.onValueChanged.RemoveListener(OnShootEnable);
            _inputShooter.shooted -= OnShoot;
        }
        void OnShoot(bool value)
        {
            if (value)
            {
                _energySlider.value -= _minEnergy;
            }
        }

        void OnShootEnable(float value)
        {
            _inputShooter.isShootEnable = value >= _minEnergy;
        }
    }
}
