using System;
using Runtime.Gameplay.Buildings.Conditions;
using Runtime.Gameplay.Buildings.General;
using Runtime.Gameplay.Buildings.UI;
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
        public EBuildTerritory BuildTerritory => _generalConfig.BuildTerritory;
        public BuildingsToolbarTabConfig ToolbarTab => _generalConfig.ToolbarTab;
        
        public string Name => _generalConfig.Name;
        public string Description => _generalConfig.Description;
        public Sprite Icon => _generalConfig.Icon;

        public BuildingUnlockCondition UnlockCondition => _unlockCondition;
        
        [SerializeField] private BuildingConfig _generalConfig;
        [SerializeField] private BuildingUnlockCondition _unlockCondition;

        public IDisposable SubscribeUnlock(DiContainer di, Action<bool> unlockCallback) =>
            _unlockCondition.Subscribe(di, unlockCallback);

        public abstract BuildingParameterEntry[] GetAdditionalParameters();

        public abstract IBuildingModel CreateModel(DiContainer container, BuildingView view);
    }
}