using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingsToolbarTabView : MonoBehaviour, IPointerClickHandler
    {
        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected;

        [SerializeField] private TextMeshProUGUI _caption;
        [SerializeField] private GameObject _selectedMark;

        private ReactiveProperty<bool> _isSelected = new(false);

        public void SetCaption(string caption) => _caption.text = caption;

        public void SetSelected(bool selected)
        {
            _selectedMark.SetActive(selected);
            _isSelected.Value = selected;
        }
        
        public void OnPointerClick(PointerEventData eventData) =>
            _isSelected.Value = true;
    }
}