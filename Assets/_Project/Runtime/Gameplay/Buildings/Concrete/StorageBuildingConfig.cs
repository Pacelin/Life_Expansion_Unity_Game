using UnityEngine;

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
    }
}