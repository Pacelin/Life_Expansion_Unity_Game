using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.Planets.UI
{
    public class PlanetInfoView : MonoBehaviour
    {
        [SerializeField] private Image _temperatureSlider;
        [SerializeField] private Image _oxygenSlider;
        [SerializeField] private Image _waterSlider;
        [SerializeField] private RectTransform _temperatureMarker;
        [SerializeField] private RectTransform _oxygenMarker;
        [SerializeField] private RectTransform _waterMarker;
        
        public void SetTemperature(float percent) =>
            _temperatureSlider.fillAmount = percent;
        public void SetOxygen(float percent) =>
            _oxygenSlider.fillAmount = percent;
        public void SetWater(float percent) =>
            _waterSlider.fillAmount = percent;

        public void SetTemperatureMarker(float percent) =>
            MoveMarkerTo(_temperatureMarker, (RectTransform) _temperatureSlider.transform, percent);
        public void SetOxygenMarker(float percent) =>
            MoveMarkerTo(_oxygenMarker, (RectTransform) _oxygenSlider.transform, percent);
        public void SetWaterMarker(float percent) =>
            MoveMarkerTo(_waterMarker, (RectTransform) _waterSlider.transform, percent);

        private void MoveMarkerTo(RectTransform marker, RectTransform parent, float percent)
        {
            Debug.Log(percent);
            var rect = parent.rect;
            var pos = marker.anchoredPosition;
            pos.x = Mathf.Lerp(0, rect.width, percent);
            marker.anchoredPosition = pos;
        }
    }
}