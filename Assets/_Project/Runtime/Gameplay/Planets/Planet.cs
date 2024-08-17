using System;
using R3;
using Runtime.Gameplay.Core;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Planets
{
    public class Planet : IInitializable, IDisposable
    {
        public Vector3 Center => _component.transform.position;
        public float Radius => _config.Radius;

        public PlanetIndicator Temperature => _temperature;
        public PlanetIndicator Oxygen => _oxygen;
        public PlanetIndicator Water => _water;

        private IDisposable _orbitalDisposable;
        
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
        }

        public void Dispose()
        {
            _temperature.Dispose();
            _oxygen.Dispose();
            _water.Dispose();
            _orbitalDisposable.Dispose();
        }
    }
}