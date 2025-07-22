using UnityEngine;

namespace PG.HealthSystem
{
    public class DamagableLinker : MonoBehaviour
    {
        [SerializeField] private GameObject _damagableObject;
        public IDamagable damagable;
        private void OnValidate()
        {
            if (_damagableObject.GetComponent<IDamagable>() == null)
            {
                _damagableObject = null;
                Debug.LogError("DamagableObject haven't interface IDamagable!");
            }
        }
        private void Awake()
        {
            _damagableObject.TryGetComponent(out damagable);
        }
    }
}
