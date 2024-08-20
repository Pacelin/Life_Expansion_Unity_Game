using System.Collections.Generic;
using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Whether Building", fileName = "Whether Building")]
    public class WhetherBuildingConfig : BuildingConditionalConfig
    {
        public float TemperatureDelta => _temperatureDelta.Value;
        public float OxygenDelta => _oxygenDelta.Value;
        public float WaterDelta => _waterDelta.Value;
        
        [SerializeField] private BuildingParameterEntry _temperatureDelta;
        [SerializeField] private BuildingParameterEntry _oxygenDelta;
        [SerializeField] private BuildingParameterEntry _waterDelta;
        
        public override BuildingParameterEntry[] GetAdditionalParameters()
        {
            var list = new List<BuildingParameterEntry>();
            if (_temperatureDelta.Value != 0)
                list.Add(_temperatureDelta);
            if (_oxygenDelta.Value != 0)
                list.Add(_oxygenDelta);
            if (_waterDelta.Value != 0)
                list.Add(_waterDelta);
            return list.ToArray();
        }

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<WhetherBuildingModel>(new object[] { this, view });
    }
}