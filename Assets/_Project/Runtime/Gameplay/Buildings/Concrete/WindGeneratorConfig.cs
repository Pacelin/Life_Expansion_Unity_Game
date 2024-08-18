using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Wind Generator")]
    public class WindGeneratorConfig : BuildingConditionalConfig
    {
        public int EnergyDelta => (int) _energyDelta.Value;
        public float OxygenDelta => _oxygenDelta.Value;
        
        [SerializeField] private BuildingParameterEntry _oxygenDelta;
        [SerializeField] private BuildingParameterEntry _energyDelta;

        public override BuildingParameterEntry[] GetAdditionalParameters() =>
            new[] { _energyDelta, _oxygenDelta };

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<WindGeneratorModel>(new object[] { this, view });
    }
}