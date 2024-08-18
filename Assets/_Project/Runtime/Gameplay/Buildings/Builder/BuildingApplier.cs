// ReSharper disable Unity.InefficientPropertyAccess

using System;
using DG.Tweening;
using Runtime.Gameplay.Planets;
using Object = UnityEngine.Object;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingApplier : IDisposable
    {
        private readonly BuildingBuilderConfig _config;
        private readonly BuildingFactory _factory;
        private readonly Planet _planet;
        private Tween _tween;
        
        public BuildingApplier(BuildingBuilderConfig config, BuildingFactory factory, Planet planet)
        {
            _config = config;
            _factory = factory;
            _planet = planet;
        }
        
        public void ValidatePosition(BuildingConditionalConfig buildingConfig, BuildingBuilderView view)
        {
            
        }
        
        public bool TryBuild(BuildingConditionalConfig buildingConfig, BuildingBuilderView view)
        {
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