using System;
using Zenject;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingsToolbarItemPresenter : IDisposable
    {
        private IDisposable _unlockDisposable;
        
        private readonly DiContainer _di;
        private readonly BuildingConditionalConfig _config;
        private readonly BuildingsToolbarItemView _view;
        
        public BuildingsToolbarItemPresenter(DiContainer di,
            BuildingConditionalConfig config, BuildingsToolbarItemView view)
        {
            _di = di;
            _config = config;
            _view = view;
        }
        
        public void Initialize()
        {
            _view.SetIcon(_config.Icon);
            _view.SetCost(_config.MineralsCost);
            _unlockDisposable = 
                _config.SubscribeUnlock(_di, unlocked => _view.SetLocked(!unlocked));
        }
        
        public void Dispose()
        {
            _unlockDisposable.Dispose();
        }
    }
}