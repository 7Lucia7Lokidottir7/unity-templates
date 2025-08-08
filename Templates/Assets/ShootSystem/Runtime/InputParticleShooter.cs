using UnityEngine;

namespace PG.ShootSystem
{
    public class InputParticleShooter : InputShooter
    {
        [SerializeField] private ParticleSystem _bulletParticleSystem;

        public override void OnShoot()
        {
            _bulletParticleSystem?.Play();
        }

        public override void OnShootEnd()
        {
            _bulletParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
