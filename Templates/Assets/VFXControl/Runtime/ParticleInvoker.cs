using System.Collections.Generic;
using UnityEngine;

namespace PG.VFXControl
{
    public class ParticleInvoker : MonoBehaviour
    {
        [SerializeField] private ParticleElement[] _particles;

        [System.Serializable]
        public struct ParticleElement
        {
            public ParticleSystem ParticleSystem;
            public string Name;
        }
        public Dictionary<string, ParticleSystem> particles;
        public ParticleSystem GetParticleSystem(int index) => _particles[index].ParticleSystem;
        public void Play(string name)
        {
            particles.TryGetValue(name, out var particle);
            particle.Play();
        }

        public void Play(int index)
        {
            _particles[index].ParticleSystem.Play();
        }

        public void Stop(string name, bool isClear = false)
        {
            particles.TryGetValue(name, out var particle);
            particle.Stop(true, isClear ? ParticleSystemStopBehavior.StopEmittingAndClear : ParticleSystemStopBehavior.StopEmitting);
        }

        public void Stop(int index, bool isClear = false)
        {
            _particles[index].ParticleSystem.Stop(true, isClear ? ParticleSystemStopBehavior.StopEmittingAndClear : ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
