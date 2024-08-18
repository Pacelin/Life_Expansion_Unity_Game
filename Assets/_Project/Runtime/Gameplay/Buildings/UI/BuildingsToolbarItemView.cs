using R3;
using Runtime.Gameplay.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingsToolbarItemView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
        public ReadOnlyReactiveProperty<bool> IsHover => _isHover;

        [SerializeField] private GameObject _selectionMark;
        [SerializeField] private GameObject _hoverMark;
        [SerializeField] private GameObject _nonPurchasableMark;
        [SerializeField] private GameObject _lockedMark;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _costText;

        private ReactiveProperty<bool> _isSelected = new();
        private ReactiveProperty<bool> _isHover = new();
        private bool _locked;

        public void SetSelected(bool selected)
        {
            _selectionMark.SetActive(selected);
            _isSelected.Value = selected;
        }

        public void SetHover(bool hover)
        {
            _hoverMark.SetActive(hover);
            _isHover.Value = hover;
        }

        public void SetIcon(Sprite icon) => _itemIcon.sprite = icon;
        public void SetCost(int cost) => _costText.text = cost.ToGameString();
        public void SetPurchasable(bool purchasable) => _nonPurchasableMark.SetActive(!purchasable);

        public void SetLocked(bool locked)
        {
            _lockedMark.SetActive(locked);
            _locked = locked;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_locked)
                return;
            SetSelected(!_isSelected.Value);
        }

        public void OnPointerEnter(PointerEventData eventData)
        { 
            if (_locked)
                return;
            SetHover(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_locked)
                return;
            SetHover(false);
        }
    }
}