using UnityEngine;

namespace PG.InteractSystem
{
    public class InteractObjectDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject[] _displayObjects;

        private void OnEnable()
        {
            IInteractVisible.visibleInteracted += OnVisible;
        }

        private void OnDisable()
        {
            IInteractVisible.visibleInteracted -= OnVisible;
        }
        void OnVisible(bool value)
        {
            foreach (var obj in _displayObjects)
            {
                obj.SetActive(value);
            }
        }
    }
}