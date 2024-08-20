using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace Runtime.Core
{
    public class TooltipPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private LocalizedString _tooltipText;
        [SerializeField] private ECursorIcon _icon;

        private bool _showing;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorTooltip.Instance?.Show(_tooltipText.GetLocalizedString(), _icon);
            _showing = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorTooltip.Instance?.Hide();
            _showing = false;
        }

        private void OnDisable()
        {
            if (_showing && CursorTooltip.Instance != null)
                CursorTooltip.Instance.Hide();
        }
    }
}