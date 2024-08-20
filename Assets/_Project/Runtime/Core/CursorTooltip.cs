using System;
using R3;
using UnityEngine;

namespace Runtime.Core
{
    public class CursorTooltip : IDisposable
    {
        public static CursorTooltip Instance => _instance;

        private static CursorTooltip _instance;
        private readonly CursorTooltipView _view;
        private IDisposable _showingDisposable;
        
        public CursorTooltip(CursorTooltipView view)
        {
            _view = view;
            _instance = this;
        }

        public void Show(string tooltip, ECursorIcon icon = ECursorIcon.No)
        {
            _view.Set(tooltip, icon);
            _view.gameObject.SetActive(true);

            _showingDisposable = Observable.EveryUpdate(UnityFrameProvider.PreLateUpdate)
                .Subscribe(_ => _view.SetPosition(Input.mousePosition));
        }

        public void Hide()
        {
            _view.gameObject.SetActive(false);
            _showingDisposable?.Dispose();
            _showingDisposable = null;
        }

        public void Dispose()
        {
            _showingDisposable?.Dispose();
            _instance = null;
        }
    }
}