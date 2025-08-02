using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace PG.VFXControl
{
    public class VisualEffectInvoker : MonoBehaviour
    {
        [SerializeField] private ParticleElement[] _visualEffects;

        [System.Serializable]
        public struct ParticleElement
        {
            public VisualEffect visualEffect;
            public string Name;
        }
        public Dictionary<string, VisualEffect> visualEffects;

        public VisualEffect GetVisualEffect(int index) => _visualEffects[index].visualEffect;
        public void Play(string name)
        {
            visualEffects.TryGetValue(name, out var visualEffect);
            visualEffect.Play();
        }

        public void Play(int index)
        {
            _visualEffects[index].visualEffect.Play();
        }

        public void Stop(string name, bool isClear = false)
        {
            visualEffects.TryGetValue(name, out var visualEffect);
            if (isClear)
            {
                visualEffect.Stop();
                visualEffect.Reinit();
            }
            else
            {
                visualEffect.Stop();
            }
        }

        public void Stop(int index, bool isClear = false)
        {
            if (isClear)
            {
                _visualEffects[index].visualEffect.Stop();
                _visualEffects[index].visualEffect.Reinit();
            }
            else
            {
                _visualEffects[index].visualEffect.Stop();
            }
        }
    }
}
