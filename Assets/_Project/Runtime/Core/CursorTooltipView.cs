using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Core
{
    public class CursorTooltipView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _additionalIcon;
        [SerializeField] private SerializedDictionary<ECursorIcon, Sprite> _icons;
        [SerializeField] private RectTransform _anchor;

        private Camera _camera;
        private RectTransform _canvasTransform;
        
        [Inject]
        private void Construct(Camera mainCamera, Canvas canvas)
        {
            _camera = mainCamera;
            _canvasTransform = (RectTransform)canvas.transform;
        }
        
        public void Set(string text, ECursorIcon icon = ECursorIcon.No)
        {
            _text.text = text;
            if (icon == ECursorIcon.No)
            {
                _additionalIcon.gameObject.SetActive(false);
            }
            else
            {
                _additionalIcon.gameObject.SetActive(true);
                _additionalIcon.sprite = _icons[icon];
            }
        }

        public void SetPosition(Vector2 screenPoint)
        {
            var viewportPoint = _camera.ScreenToViewportPoint(screenPoint);
            var canvasRect = _canvasTransform.rect;
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
    }
}