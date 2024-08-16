using UniOwl.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniOwl.UI
{
    public class UniSelectableAudio : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        [SerializeField] private Selectable _selectable;
        
        [SerializeField] private AudioContainer _hoverAudio, _clickAudio;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_selectable != null && !_selectable.interactable) return;
            AudioSFXSystem.PlayCue2D(_hoverAudio);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_selectable != null && !_selectable.interactable) return;
            AudioSFXSystem.PlayCue2D(_clickAudio);
        }
    }
}