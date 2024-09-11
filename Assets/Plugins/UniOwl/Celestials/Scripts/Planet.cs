using UnityEngine;

namespace UniOwl.Celestials
{
    public class Planet : MonoBehaviour
    {
        [SerializeField]
        private PlanetObject _planetObject;
        
        private float temperatureLevel, atmosphereLevel, overallLevel, waterLevel;
        
        /// <param name="temperatureLevel">Range [0, 1], 0 = very cold, 1 = very hot, 0.5 = normal temperature.</param>
        public void SetTemperatureLevel(float temperatureLevel)
        {
            this.temperatureLevel = temperatureLevel;
            UpdatePlanetAppearance();
        }
        
        /// <param name="atmosphereLevel">Range [0, 1], 0 = no atmosphere, 1 = dense atmosphere.</param>
        public void SetAtmosphereLevel(float atmosphereLevel)
        {
            this.atmosphereLevel = atmosphereLevel;
            UpdatePlanetAppearance();
        }

        /// <param name="overallLevel">Range [0, 1], 0 = no life, 1 = full life.</param>
        public void SetOverallLevel(float overallLevel)
        {
            this.overallLevel = overallLevel;
            UpdatePlanetAppearance();
        }

        /// <param name="waterLevel">Range [0, 2], 0 = no water, 1 = flooded world.</param>
        public void SetWaterLevel(float waterLevel)
        {
            this.waterLevel = waterLevel;
            UpdatePlanetAppearance();
        }
        
        private void UpdatePlanetAppearance()
        {
            for (int i = 0; i < _planetObject.Components.Length; i++)
            {
                GameObject editableGO = transform.GetChild(i).gameObject;
                _planetObject.Components[i].UpdateVisual(editableGO);
            }
        }
    }
}
