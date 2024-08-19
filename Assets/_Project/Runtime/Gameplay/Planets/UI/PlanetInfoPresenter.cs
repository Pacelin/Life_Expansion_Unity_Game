using System;
using Zenject;
using R3;
using Runtime.Gameplay.Colonizers;

namespace Runtime.Gameplay.Planets.UI
{
    public class PlanetInfoPresenter : IInitializable, IDisposable
    {
        private readonly ColonizersModel _colonizers;
        private readonly Planet _model;
        private readonly PlanetInfoView _view;
        private readonly CompositeDisposable _disposables;
        
        public PlanetInfoPresenter(ColonizersModel colonizers, Planet model, PlanetInfoView view)
        {
            _colonizers = colonizers;
            _model = model;
            _view = view;
            _disposables = new();
        }
        
        public void Initialize()
        {
            _view.SetTemperatureMarker(_colonizers.TargetTemperature);
            _view.SetOxygenMarker(_colonizers.TargetOxygen);
            _view.SetWaterMarker(_colonizers.TargetWater);
            
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