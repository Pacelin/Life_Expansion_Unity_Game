// ReSharper disable Unity.InefficientPropertyAccess

using System;
using DG.Tweening;
using Runtime.Gameplay.Colonizers;
using Runtime.Gameplay.Colonizers.UI;
using Runtime.Gameplay.Planets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingApplier : IDisposable
    {
        private readonly BuildingBuilderConfig _config;
        private readonly BuildingFactory _factory;
        private readonly Planet _planet;
        private readonly ColonizersModel _colonizers;
        private readonly ColonizersInfoPresenter _colonizersInfoPresenter;
        private bool _canBuild;
        private Tween _tween;
        
        public BuildingApplier(BuildingBuilderConfig config, BuildingFactory factory,
            Planet planet, ColonizersModel colonizers, ColonizersInfoPresenter colonizersInfoPresenter)
        {
            _config = config;
            _factory = factory;
            _planet = planet;
            _colonizers = colonizers;
            _colonizersInfoPresenter = colonizersInfoPresenter;
        }
        
        public void ValidatePosition(BuildingConditionalConfig buildingConfig, BuildingBuilderView view, bool isOnWater)
        {
            bool canBuildOnTerritory = (buildingConfig.BuildTerritory == EBuildTerritory.Water && isOnWater) ||
                                       (buildingConfig.BuildTerritory == EBuildTerritory.Ground && !isOnWater);
            
            _canBuild = canBuildOnTerritory && !Physics.CheckSphere(view.transform.position, 
                buildingConfig.Prefab.BuildingRadius, _config.BuildLayer);
            
            view.SetProjector(_canBuild);
        }
        
        public bool TryBuild(BuildingConditionalConfig buildingConfig, BuildingBuilderView view)
        {
            if (!_canBuild)
                return false;
            if (!_colonizers.Minerals.HasMinerals(buildingConfig.MineralsCost))
            {
                _colonizersInfoPresenter.PingMinerals();
                return false;
            }
            var position = view.transform.position;
            var up = view.transform.up;
            var startPosition = position + up * _config.BuildAnimationStartDistance;

            var building = Object.Instantiate(buildingConfig.Prefab, startPosition, view.transform.rotation, _planet.Transform);
            _tween = building.transform.DOMove(position, _config.BuildAnimationDuration)
                .SetEase(_config.BuildAnimationCurve);
            _factory.Add(buildingConfig, building);
            return true;
        }

        public void Dispose()
        {
            _tween?.Kill();
        }
    }
}