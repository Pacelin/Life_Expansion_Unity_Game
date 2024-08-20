using System;
using R3;
using Runtime.Gameplay.Colonizers;
using Zenject;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingsToolbarItemPresenter : IDisposable
    {
        private readonly DiContainer _di;
        private readonly ColonizersModel _colonizers;
        private readonly BuildingConditionalConfig _config;
        private readonly BuildingsToolbarItemView _view;
        private readonly CompositeDisposable _disposables;
        
        public BuildingsToolbarItemPresenter(DiContainer di, ColonizersModel colonizers,
            BuildingConditionalConfig config, BuildingsToolbarItemView view)
        {
            _di = di;
            _colonizers = colonizers;
            _config = config;
            _view = view;
            _disposables = new();
        }
        
        public void Initialize()
        {
            _view.SetIcon(_config.Icon);
            _view.SetCost(_config.MineralsCost);
            if (_config.UnlockCondition.HasDescription)
                _view.SetTooltipText(_config.UnlockCondition.LockingDescription);
            _colonizers.Minerals.CurrentMinerals
                .Subscribe(minerals => _view.SetPurchasable(minerals >= _config.MineralsCost))
                .AddTo(_disposables);
            _config.SubscribeUnlock(_di, unlocked => _view.SetLocked(!unlocked))
                .AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}