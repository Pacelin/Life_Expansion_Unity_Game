using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.Planets.UI
{
    public class PlanetInfoView : MonoBehaviour
    {
        [SerializeField] private Slider _temperatureSlider;
        [SerializeField] private Slider _oxygenSlider;
        [SerializeField] private Slider _waterSlider;

        public void SetTemperature(float percent) =>
            _temperatureSlider.value = percent;
        public void SetOxygen(float percent) =>
            _oxygenSlider.value = percent;
        public void SetWater(float percent) =>
            _waterSlider.value = percent;
    }
}