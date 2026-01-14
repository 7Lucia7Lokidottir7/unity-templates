using UnityEngine;
using UnityEngine.EventSystems;

namespace PG.MenuManagement
{
    public class SoundSelectable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, ISelectHandler, ISubmitHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (UIAudioManager.Instance != null)
                UIAudioManager.Instance.PlayClick();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (UIAudioManager.Instance != null)
                UIAudioManager.Instance.PlayHover();
        }

        public void OnSelect(BaseEventData eventData)
        {
            // Часто при клике срабатывает и Select, можно оставить пустым, если звуки дублируются
            if (UIAudioManager.Instance != null)
                UIAudioManager.Instance.PlaySelect();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (UIAudioManager.Instance != null)
                UIAudioManager.Instance.PlayClick();
        }
    }
}