using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Living House", fileName = "Living House")]
    public class LivingHouseConfig : BuildingConditionalConfig
    {
        public int ColonizersMax => (int) _colonizersMaxDelta.Value;
        
        [SerializeField] private BuildingParameterEntry _colonizersMaxDelta;

        public override BuildingParameterEntry[] GetAdditionalParameters() =>
            new[] { _colonizersMaxDelta };

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<LivingHouseModel>(new object[] { this, view });
    }
}