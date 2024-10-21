using Runtime.Cinematic;
using Runtime.Gameplay.Buildings.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingsToolbarItemViewAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private BuildingsToolbarItemView _view;
    
    [SerializeField] private AudioClip _enterSound;
    [SerializeField] private float _enterSoundVolume = 1;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private float _clickSoundVolume = 1;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_view && _view.Locked) return;
        
        Audio.PlaySound(_enterSound, _enterSoundVolume);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_view && _view.Locked) return;

        Audio.PlaySound(_clickSound, _clickSoundVolume);
    }
}
