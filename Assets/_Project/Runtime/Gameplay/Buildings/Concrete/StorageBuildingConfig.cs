using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Storage Config")]
    public class StorageBuildingConfig : BuildingConditionalConfig
    {
        public int IncreaseStorage => (int) _increaseStorageParameter.Value;
        
        [SerializeField] private BuildingParameterEntry _increaseStorageParameter;


        public override BuildingParameterEntry[] GetAdditionalParameters()
        {
            return new[] { _increaseStorageParameter };
        }

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<StorageBuildingModel>(new object[] { this, view });
    }
}