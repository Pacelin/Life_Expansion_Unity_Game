using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Hydro Generator", fileName = "Hydro Generator")]
    public class HydroGeneratorConfig : BuildingConditionalConfig
    {
        public int EnergyDelta => (int) _energyDelta.Value;
        public float WaterDelta => _waterDelta.Value;
        
        [SerializeField] private BuildingParameterEntry _waterDelta;
        [SerializeField] private BuildingParameterEntry _energyDelta;

        public override BuildingParameterEntry[] GetAdditionalParameters() =>
            new[] { _energyDelta, _waterDelta };

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<HydroGeneratorModel>(new object[] { this, view });
    }
}