using R3;
using Runtime.Gameplay.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Gameplay.Toolbar
{
    public class ToolbarItemView : MonoBehaviour, IPointerClickHandler
    {
        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
        
        [SerializeField] private GameObject _selectionMark;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _costText;

        private ReactiveProperty<bool> _isSelected = new();

        public void SetSelected(bool selected)
        {
            _selectionMark.SetActive(selected);
            _isSelected.Value = selected;
        }

        public void SetIcon(Sprite icon) => _itemIcon.sprite = icon;
        public void SetCost(int cost) => _costText.text = cost.ToGameString();
        
        public void OnPointerClick(PointerEventData eventData) =>
            SetSelected(!_isSelected.Value);
    }
}