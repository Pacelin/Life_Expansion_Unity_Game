using System;
using R3;
using Runtime.Gameplay.Colonizers;
using Zenject;

namespace Runtime.Gameplay.Planets
{
    public class PlanetSync : IInitializable, IDisposable
    {
        [Inject] private Planet _planet;
        [Inject] private ColonizersModel _colonizers;

        private CompositeDisposable _disposables;
        
        public void Initialize()
        {
            _disposables = new();
            _colonizers.Feeling.Subscribe(f =>
                _planet.UniPlanet.SetOverallLevel(f))
                .AddTo(_disposables);
            _planet.Oxygen.NormalizedValue.Subscribe(f =>
                _planet.UniPlanet.SetAtmosphereLevel(f))
                .AddTo(_disposables);
            _planet.Temperature.NormalizedValue.Subscribe(f =>
                _planet.UniPlanet.SetTemperatureLevel(f))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}