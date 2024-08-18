using System;
using System.Collections.Generic;
using Runtime.Gameplay.Buildings.Conditions;
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

        public abstract BuildingParameterEntry[] GetAdditionalParameters();
    }
}