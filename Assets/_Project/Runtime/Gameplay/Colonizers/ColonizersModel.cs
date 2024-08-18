using System;
using System.Linq;
using R3;
using Runtime.Gameplay.Core;
using Runtime.Gameplay.Planets;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Colonizers
{
    public class ColonizersModel : IInitializable, IDisposable
    {
        public ColonizersMineralsModel Minerals => _minerals;
        public ColonizersPopulationModel Population => _population;
        public ColonizersEnergyModel Energy => _energy;
        
        private IDisposable _tickDisposable;
            
        private readonly GameplayCore _gameplayCore;
        
        private readonly ColonizersIndicatorModel _temperature;
        private readonly ColonizersIndicatorModel _oxygen;
        private readonly ColonizersIndicatorModel _water;
        private readonly ColonizersPopulationModel _population;
        private readonly ColonizersMineralsModel _minerals;
        private readonly ColonizersEnergyModel _energy;
        private readonly ReadOnlyReactiveProperty<float> _feeling;

        public ColonizersModel(ColonizersConfig config, Planet planet, GameplayCore gameplayCore)
        {
            _gameplayCore = gameplayCore;
            _temperature = new ColonizersIndicatorModel(config.Temperature, planet.Temperature);
            _oxygen = new ColonizersIndicatorModel(config.Oxygen, planet.Oxygen);
            _water = new ColonizersIndicatorModel(config.Water, planet.Water);
            _population = new ColonizersPopulationModel(config);
            _minerals = new ColonizersMineralsModel(config.Capsule.InitialMinerals, config.Capsule.InitialMaxMinerals);
            _energy = new ColonizersEnergyModel(config.Capsule.InitialEnergy);
            _feeling = Observable.CombineLatest(_temperature.Feeling, _oxygen.Feeling, _water.Feeling)
                .Select(values => values.Average()).ToReadOnlyReactiveProperty();
        }

        public void Initialize()
        {
            _tickDisposable = _gameplayCore.OnTick
                .Subscribe(delta =>
                {
                    var targetPopulation = Mathf.Lerp(0, _population.Ideal, _feeling.CurrentValue);
                    _population.Set(Mathf.Lerp(_population.CurrentPopulation.CurrentValue, targetPopulation, 
                        _population.Interpolation * delta));
                });
        }
        
        public void Dispose()
        {
            _tickDisposable.Dispose();
        }
    }
}