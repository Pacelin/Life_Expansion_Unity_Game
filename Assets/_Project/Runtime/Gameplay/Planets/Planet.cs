using System;
using R3;
using Runtime.Gameplay.Buildings;
using Runtime.Gameplay.Core;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Planets
{
    public class Planet : IInitializable, IDisposable
    {
        public Transform Transform => _component.transform;
        public Vector3 Center => _component.transform.position;
        public float Radius => _config.MaxRadius;

        public PlanetIndicator Temperature => _temperature;
        public PlanetIndicator Oxygen => _oxygen;
        public PlanetIndicator Water => _water;
        public UniOwl.Celestials.Planet_Old UniPlanet => _component.UniPlanet;

        private IDisposable _orbitalDisposable;
        private IDisposable _waterDisposable;
        
        private readonly GameplayCore _gameplayCore;
        private readonly PlanetConfig _config;
        private readonly PlanetComponent _component;
        
        private readonly PlanetIndicator _temperature;
        private readonly PlanetIndicator _oxygen;
        private readonly PlanetIndicator _water;
        
        public Planet(GameplayCore gameplayCore, PlanetConfig config, PlanetComponent component)
        {
            _gameplayCore = gameplayCore;
            _config = config;
            _component = component;
            _temperature = new PlanetIndicator(_config.Temperature);
            _oxygen = new PlanetIndicator(_config.Oxygen);
            _water = new PlanetIndicator(_config.Water);
        }

        public void Initialize()
        {
            _temperature.Initialize(_gameplayCore);
            _oxygen.Initialize(_gameplayCore);
            _water.Initialize(_gameplayCore);
            _orbitalDisposable = Observable.EveryUpdate(UnityFrameProvider.Update)
                .Subscribe(_ => _component.transform.Rotate(_component.transform.up, _config.OrbitalSpeed * Time.deltaTime));
            _waterDisposable = _water.NormalizedValueUnclamped.Subscribe(normalized =>
                _component.SetWaterRadius(_config.MinRadius + (_config.MaxRadius - _config.MinRadius) * normalized));
        }

        public void Dispose()
        {
            _temperature.Dispose();
            _oxygen.Dispose();
            _water.Dispose();
            _orbitalDisposable.Dispose();
            _waterDisposable.Dispose();
        }
    }
}