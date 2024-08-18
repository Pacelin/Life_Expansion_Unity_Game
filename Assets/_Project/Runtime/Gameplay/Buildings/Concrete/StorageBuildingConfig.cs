using Runtime.Gameplay.Buildings.Conditions;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Storage Config")]
    public class StorageBuildingConfig : BuildingConditionalConfig
    {
        public override string Description => string.Format(base.Description, _increaseStorageCapacity);
        public int IncreaseStorageCapacity => _increaseStorageCapacity;
        
        [Header("{0}")]
        [SerializeField] private int _increaseStorageCapacity;
    }
}