using R3;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Gameplay.Buildings.General
{
    public class BuildingInteractorView : MonoBehaviour
    {
        public Observable<Unit> OnClick => _button.OnClickAsObservable();
        public string DestroyString => _destroyString.GetLocalizedString();

        [SerializeField] private LocalizedString _destroyString;
        [SerializeField] private RectTransform _anchor;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        
        [Inject] private Camera _camera;
        [Inject] private Canvas _canvas;

        public void ShowMessage(string text, Vector3 screenPoint)
        {
            _text.text = text;
            gameObject.SetActive(true);
            
            var viewportPoint = _camera.ScreenToViewportPoint(screenPoint);
            var canvasRect = ((RectTransform) _canvas.transform).rect;
            var canvasPoint =new Vector2(
                viewportPoint.x * canvasRect.width,
                viewportPoint.y * canvasRect.height);
            var distanceToRight = canvasRect.width - canvasPoint.x;
            var distanceToTop = canvasRect.height - canvasPoint.y;

            var rect = _anchor.rect;
            var height = rect.height;
            var width = rect.width;

            var x = distanceToRight < width ? 1 : 0;
            var y = distanceToTop < height ? 1 : 0;
            _anchor.pivot = new Vector2(x, y);
            _anchor.anchoredPosition = canvasPoint;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}