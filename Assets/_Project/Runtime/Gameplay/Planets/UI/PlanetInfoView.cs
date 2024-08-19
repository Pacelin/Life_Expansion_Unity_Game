using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.Planets.UI
{
    public class PlanetInfoView : MonoBehaviour
    {
        [SerializeField] private Image _temperatureSlider;
        [SerializeField] private Image _oxygenSlider;
        [SerializeField] private Image _waterSlider;

        public void SetTemperature(float percent) =>
            _temperatureSlider.fillAmount = percent;
        public void SetOxygen(float percent) =>
            _oxygenSlider.fillAmount = percent;
        public void SetWater(float percent) =>
            _waterSlider.fillAmount = percent;
    }
}