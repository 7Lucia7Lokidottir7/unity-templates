using UnityEngine;
using UnityEngine.Events;
namespace PG.InteractSystem
{
    public class InteractObjectTrigger : MonoBehaviour, IInteractable
    {
        public event System.Action interacted;
        private const string _PLAYER_TAG = "Player";

        [field: SerializeField] public UnityEvent interactEvent { get; set; }
        public void OnInteract()
        {
            interacted?.Invoke();
            interactEvent?.Invoke();

        }
        private void OnDisable()
        {
            if (TriggerInteractManager.instance != null)
            {
                TriggerInteractManager.instance.RemoveTrigger(this);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (TriggerInteractManager.instance == null)
            {
                return;
            }

            if (other.CompareTag(_PLAYER_TAG))
            {
                TriggerInteractManager.instance?.AddTrigger(this);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (TriggerInteractManager.instance == null)
            {
                return;
            }

            if (other.CompareTag(_PLAYER_TAG))
            {
                TriggerInteractManager.instance?.RemoveTrigger(this);
            }
        }
    }
}
