using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PG.MenuManagement
{
    public class SelectableCell : Selectable, ISubmitHandler, ICancelHandler
    {
        [SerializeField] private Selectable _cancelSelectable;
        [SerializeField] private Selectable _submitSelectable;
        public UnityEvent cancelEvent;
        public UnityEvent submitEvent;
        public void OnCancel(BaseEventData eventData)
        {
            _cancelSelectable?.Select();
            cancelEvent?.Invoke();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            _submitSelectable?.Select();
            submitEvent?.Invoke();
        }
    }
}
