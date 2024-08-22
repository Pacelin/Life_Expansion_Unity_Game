using Runtime.Cinematic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.MainMenu
{
    public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private AudioClip _enterSound;
        [SerializeField] private float _enterSoundVolume = 1;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private float _clickSoundVolume = 1;
        
        public void OnPointerEnter(PointerEventData eventData) =>
            Audio.PlaySound(_enterSound, _enterSoundVolume);
        public void OnPointerClick(PointerEventData eventData) =>
            Audio.PlaySound(_clickSound, _clickSoundVolume);
    }
}