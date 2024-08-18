using System;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings
{
    public abstract class BuildingConditionalConfig : ScriptableObject
    {
        public BuildingView Prefab => _generalConfig.Prefab;

        public int MineralsCost => _generalConfig.MineralsCost;
        public int ColonizersCost => _generalConfig.ColonizersCost;
        public int EnergyCost => _generalConfig.EnergyCost;
        
        public string Name => _generalConfig.Name;
        public virtual string Description => _generalConfig.Description;
        public Sprite Icon => _generalConfig.Icon;
        
        [SerializeField] private BuildingConfig _generalConfig;
        [SerializeField] private BuildingUnlockCondition _unlockCondition;

        public IDisposable SubscribeUnlock(DiContainer di, Action<bool> unlockCallback) =>
            _unlockCondition.Subscribe(di, unlockCallback);
    }
    
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Storage Config")]
    public class StorageBuildingConfig : BuildingConditionalConfig
    {
        public override string Description => string.Format(base.Description, _increaseStorageCapacity);
        public int IncreaseStorageCapacity => _increaseStorageCapacity;
        
        [Header("{0}")]
        [SerializeField] private int _increaseStorageCapacity;
    }
}