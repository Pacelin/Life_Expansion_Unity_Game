using System;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Colonizers.UI
{
    public class ColonizersInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _mineralsText;
        [SerializeField] private string _mineralsFormat;
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private string _energyFormat;
        [SerializeField] private TextMeshProUGUI _populationText;
        [SerializeField] private string _populationFormat;
        [SerializeField] private TextMeshProUGUI _targetPopulationText;
        [SerializeField] private GameObject _targetReachedMark;
        
        public void SetMinerals(int current, int max) =>
            _mineralsText.text = string.Format(_mineralsFormat, current, max);

        public void SetEnergy(int usage, int max) =>
            _energyText.text = string.Format(_energyFormat, usage, max);

        public void SetTargetReached(bool reached) =>
            _targetReachedMark.SetActive(reached);
        
        public void SetPopulation(int current, int max) =>
            _populationText.text = string.Format(_populationFormat, current, max);

        public void SetTargetPopulation(int target) =>
            _targetPopulationText.text = target.ToString();
    }

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

        public void Initialize()
        {
            Observable.CombineLatest(_model.Minerals.CurrentMinerals, _model.Minerals.MaxMinerals,
                    (Current, Max) => (Current, Max))
                .Subscribe(pair => _view.SetMinerals(pair.Current, pair.Max))
                .AddTo(_disposables);
            Observable.CombineLatest(_model.Population.CurrentPopulation, _model.Population.MaxPopulation,
                    (Current, Max) => (Current, Max))
                .Subscribe(pair => _view.SetPopulation(pair.Current, pair.Max))
                .AddTo(_disposables);
            /*_model.
            _targetReachedDisposable = _model.Population.IsTargetCompleted.
            _model.Population.IsTargetCompleted
                .
                .Subscribe()
            
            _view.SetTargetPopulation(_model.Population.Target);
                
            _model.Population.
            //*/
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}