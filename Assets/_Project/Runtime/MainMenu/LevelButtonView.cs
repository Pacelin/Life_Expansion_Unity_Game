using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Runtime.MainMenu
{
    public class LevelButtonView : MonoBehaviour
    {
        public Observable<Unit> OnClick => _button.OnClickAsObservable();
        
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _caption;
        private LocalizedString _captionString;

        private void OnEnable()
        {
            if (_captionString != null)
                _captionString.StringChanged += CaptionStringOnStringChanged;
        }

        private void OnDisable()
        {
            if (_captionString != null)
                _captionString.StringChanged -= CaptionStringOnStringChanged;
        }

        private void CaptionStringOnStringChanged(string value) =>
            _caption.text = value;

        public void Set(Sprite icon, LocalizedString caption)
        {
            _icon.sprite = icon;
            _caption.text = caption.GetLocalizedString();
            _captionString = caption;
            if (gameObject.activeInHierarchy)
                OnEnable();
        }
    }
}