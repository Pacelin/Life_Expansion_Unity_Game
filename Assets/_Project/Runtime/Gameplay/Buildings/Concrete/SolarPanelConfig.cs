using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Solar Panel", fileName = "Solar Panel")]
    public class SolarPanelConfig : BuildingConditionalConfig
    {
        public int EnergyDelta => (int) _energyDelta.Value;
        
        [SerializeField] private BuildingParameterEntry _energyDelta;

        public override BuildingParameterEntry[] GetAdditionalParameters() =>
            new[] { _energyDelta };

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<SolarPanelModel>(new object[] { this, view });
    }
}