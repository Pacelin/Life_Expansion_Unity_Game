using System;
using Zenject;
using R3;

namespace Runtime.Gameplay.Planets.UI
{
    public class PlanetInfoPresenter : IInitializable, IDisposable
    {
        private readonly Planet _model;
        private readonly PlanetInfoView _view;
        private readonly CompositeDisposable _disposables;
        
        public PlanetInfoPresenter(Planet model, PlanetInfoView view)
        {
            _model = model;
            _view = view;
            _disposables = new();
        }
        
        public void Initialize()
        {
            _model.Temperature.NormalizedValue
                .Subscribe(t => _view.SetTemperature(t))
                .AddTo(_disposables);
            _model.Oxygen.NormalizedValue
                .Subscribe(o => _view.SetOxygen(o))
                .AddTo(_disposables);
            _model.Water.NormalizedValue
                .Subscribe(w => _view.SetWater(w))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}