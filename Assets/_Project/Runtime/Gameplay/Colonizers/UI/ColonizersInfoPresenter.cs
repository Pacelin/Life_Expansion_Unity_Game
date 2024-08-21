using System;
using R3;
using Zenject;

namespace Runtime.Gameplay.Colonizers.UI
{
    public class ColonizersInfoPresenter : IInitializable, IDisposable
    {
        private readonly IDisposable _targetReachedDisposable;
        private readonly ColonizersModel _model;
        private readonly ColonizersInfoView _view;
        private readonly CompositeDisposable _disposables;

        public ColonizersInfoPresenter(ColonizersModel model, ColonizersInfoView view)
        {
            _model = model;
            _view = view;
            _disposables = new();
        }

        public void PingMinerals()
        {
            _view.PingMinerals();
        }
        
        public void Initialize()
        {
            _model.Minerals.CurrentMinerals
                .Subscribe(m => _view.SetMinerals(m))
                .AddTo(_disposables);
            _model.Minerals.MaxMinerals
                .Subscribe(m => _view.SetMaxMinerals(m))
                .AddTo(_disposables);

            Observable.CombineLatest(_model.Energy.Energy, _model.Energy.EnergyUsage,
                    (Overall, Usage) => (Overall, Usage))
                .Subscribe(pair =>
                {
                    _view.SetEnergy(pair.Overall - pair.Usage);
                    _view.SetEnergyUsage(pair.Usage);
                })
                .AddTo(_disposables);
            
            Observable.CombineLatest(_model.Population.CurrentPopulation, _model.Population.BusyPopulation,
                    (Current, Busy) => (Current, Busy))
                .Subscribe(pair =>
                {
                    _view.SetPopulation((int)pair.Current - pair.Busy, (int)pair.Current);
                    if (!_model.Population.IsTargetCompleted.CurrentValue)
                        _view.SetTargetProgress(1f * _model.Population.CurrentPopulation.CurrentValue / _model.Population.Target);
                })
                .AddTo(_disposables);
            _model.Population.MaxPopulation
                .Subscribe(m => _view.SetPopulationCapacity(m))
                .AddTo(_disposables);
            _model.Population.IsTargetCompleted
                .Subscribe(isCompleted => _view.SetTargetReached(isCompleted))
                .AddTo(_disposables);
            _view.SetTargetPopulation(_model.Population.Target);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}