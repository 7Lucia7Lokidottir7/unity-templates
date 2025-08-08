using PG.HealthSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PG.BattleSystem
{
    public class DeathExpIncreaser : MonoBehaviour
    {
        [SerializeField] private GameObject _deathObject;
        private IDeath _death;
        [SerializeField] private float _exp;
        [SerializeField] private AssetReferenceGameObject _uiObject;
        [SerializeField] private float _expGameObjectTime = 1f;
        private void Awake()
        {
            _deathObject.TryGetComponent(out _death);
        }
        private void OnEnable()
        {
            _death.dead += AddExp;
        }
        private void OnDisable()
        {
            _death.dead -= AddExp;
        }
        void AddExp()
        {
            Statistics.instance.exp += _exp;
            _uiObject.InstantiateAsync().Completed += operation =>
            {
                Statistics.instance.StartCoroutine(ReleaseUI(operation.Result));
            };
        }   
        IEnumerator ReleaseUI(GameObject gameObjectUI)
        {
            yield return new WaitForSecondsRealtime(_expGameObjectTime);
            _uiObject.ReleaseInstance(gameObjectUI);
        }
    }
}
