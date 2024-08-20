using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Geothermal Generator", fileName = "Geothermal Generator")]
    public class GeothermalGeneratorConfig : BuildingConditionalConfig
    {
        public int EnergyDelta => (int) _energyDelta.Value;
        public float TemperatureDelta => _temperatureDelta.Value;
        
        [SerializeField] private BuildingParameterEntry _temperatureDelta;
        [SerializeField] private BuildingParameterEntry _energyDelta;

        public override BuildingParameterEntry[] GetAdditionalParameters() =>
            new[] { _energyDelta, _temperatureDelta };

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<GeothermalGeneratorModel>(new object[] { this, view });
    }
}