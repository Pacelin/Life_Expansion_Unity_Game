using System;
using R3;
using Runtime.Core;
using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class MineBuildingModel : BuildingModel<MineBuildingConfig>
    {
        private bool _timerFinished;
        private IDisposable _timerDisposable;
        private IDisposable _collectDisposable;
        
        protected override void OnEnable()
        {
            _planet.Oxygen.ApplyDelta(_config.OxygenDelta);
            _timerFinished = false;
            
            _timerDisposable =
                Observable.Timer(TimeSpan.FromSeconds(_config.ProductionSeconds), UnityTimeProvider.Update)
                .Subscribe(_ =>
                {
                    _timerFinished = true;
                    _view.Bubble.ShowBubble(EBubbleIcon.Minerals, _bubbleConfig.CollectText, ECursorIcon.No);
                });
            _collectDisposable =
                _view.Bubble.OnClick.Where(_ => _timerFinished)
                    .Subscribe(_ =>
                    {
                        _timerFinished = false;
                        _colonizers.Minerals.AddMinerals(_config.Production, out var _);
                        _timerDisposable = 
                            Observable.Timer(TimeSpan.FromSeconds(_config.ProductionSeconds), UnityTimeProvider.Update)
                                .Subscribe(_ =>
                                {
                                    _timerFinished = true;
                                    _view.Bubble.ShowBubble(EBubbleIcon.Minerals, _bubbleConfig.CollectText, ECursorIcon.No);
                                });
                    });
        }

        protected override void OnDisable()
        {
            _planet.Oxygen.ApplyDelta(-_config.OxygenDelta);
            
            _timerFinished = false;
            _timerDisposable?.Dispose();
            _collectDisposable?.Dispose();
            _timerDisposable = null;
            _collectDisposable = null;
        }

        protected override void Dispose()
        {
            _timerDisposable?.Dispose();
            _collectDisposable?.Dispose();
            _timerDisposable = null;
            _collectDisposable = null;
        }
    }
}