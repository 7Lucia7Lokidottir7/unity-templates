using UnityEngine;

namespace PG.InteractSystem
{
    public class InteractObjectDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject[] _displayObjects;

        private void OnEnable()
        {
            IInteractable.visibleInteracted += OnVisible;
        }

        private void OnDisable()
        {
            IInteractable.visibleInteracted -= OnVisible;
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