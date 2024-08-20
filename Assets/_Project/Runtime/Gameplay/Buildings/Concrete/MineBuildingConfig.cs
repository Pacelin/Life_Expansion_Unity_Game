using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Mine Building", fileName = "Mine Building")]
    public class MineBuildingConfig : BuildingConditionalConfig
    {
        public int Production => (int) _production.Value;
        public float ProductionSeconds => _productionTime.Value;
        public float OxygenDelta => _oxygenDelta.Value;
        
        [SerializeField] private BuildingParameterEntry _oxygenDelta;
        [SerializeField] private BuildingParameterEntry _production;
        [SerializeField] private BuildingParameterEntry _productionTime;

        public override BuildingParameterEntry[] GetAdditionalParameters() =>
            new[] { _production, _oxygenDelta };

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<MineBuildingModel>(new object[] { this, view });
    }
}