using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Storage Config")]
    public class StorageBuildingConfig : BuildingConditionalConfig
    {
        [SerializeField] private BuildingParameterEntry _increaseStorageParameter;


        public override BuildingParameterEntry[] GetAdditionalParameters()
        {
            return new[] { _increaseStorageParameter };
        }

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<BuildingStorageModel>(new object[] { this, view });
    }

    public class BuildingStorageModel : BuildingModel<StorageBuildingConfig>
    {
        protected override void EnableBuilding()
        {
            
        }

        protected override void DisableBuilding()
        {
        }
    }
}