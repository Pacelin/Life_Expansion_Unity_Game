using AYellowpaper.SerializedCollections;
using R3;
using Runtime.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Gameplay.Buildings
{
    public class BuildingBubbleView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Observable<Unit> OnClick => _onClick;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _icon;
        [SerializeField] private SerializedDictionary<EBubbleIcon, Sprite> _icons;

        private ReactiveCommand<Unit> _onClick = new();
        private string _tooltip;
        private ECursorIcon _cursorIcon;
        private bool _tooltipShown;
        private Camera _lookAt;

        public void SetCamera(Camera lookAtCamera)
        {
            _lookAt = lookAtCamera;
            _canvas.worldCamera = lookAtCamera;
        }

        private void Update()
        {
            if (!_lookAt)
                return;
            Quaternion lookRotation = _lookAt.transform.rotation;
            transform.rotation = lookRotation;
        }

        public void ShowBubble(EBubbleIcon icon, string tooltip, ECursorIcon cursorIcon)
        {
            gameObject.SetActive(true);
            _icon.sprite = _icons[icon];
            _tooltip = tooltip;
            _cursorIcon = cursorIcon;
            if (_tooltipShown)
            {
                CursorTooltip.Instance.Hide();
                CursorTooltip.Instance.Show(_tooltip, _cursorIcon);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            if (_tooltipShown)
            {
                CursorTooltip.Instance.Hide();
                _tooltipShown = false;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorTooltip.Instance.Show(_tooltip, _cursorIcon);
            _tooltipShown = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_tooltipShown)
            {
                CursorTooltip.Instance.Hide();
                _tooltipShown = false;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData) => _onClick.Execute(Unit.Default);
    }
}