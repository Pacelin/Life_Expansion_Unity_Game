using System;
using Runtime.Gameplay.Planets;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildService
    {
        [Inject] private Planet _planet;
        [Inject] private BuildingBuilderConfig _config;
        
        public EBuildTerritory CheckBuildTerritory(Vector3 position, out Vector3 hitPoint)
        {
            var direction = (_planet.Center - position).normalized;
            var ray = new Ray(position - direction * 10f, direction);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, _config.PlanetLayer))
            {
                hitPoint = hit.point;
                if (hit.collider.CompareTag(_config.WaterTag))
                    return EBuildTerritory.Water;
                return EBuildTerritory.Ground;
            }

            throw new Exception("Unknown Territory");
        }
    }
}