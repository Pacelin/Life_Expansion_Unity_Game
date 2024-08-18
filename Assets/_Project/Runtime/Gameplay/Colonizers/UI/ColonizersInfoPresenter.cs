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
            Observable.CombineLatest(_model.Minerals.CurrentMinerals, _model.Minerals.MaxMinerals,
                    (Current, Max) => (Current, Max))
                .Subscribe(pair => _view.SetMinerals(pair.Current, pair.Max))
                .AddTo(_disposables);
            Observable.CombineLatest(_model.Population.CurrentPopulation, _model.Population.MaxPopulation,
                    (Current, Max) => (Current, Max))
                .Subscribe(pair =>
                {
                    _view.SetPopulation(pair.Current, pair.Max);
                    if (!_model.Population.IsTargetCompleted.CurrentValue)
                        _view.SetTargetProgress(1f * _model.Population.CurrentPopulation.CurrentValue / _model.Population.Target);
                })
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