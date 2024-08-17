using System;
using R3;
using Zenject;

namespace Runtime.Gameplay.Core
{
    public class GameplayCore : IInitializable, IDisposable
    {
        public Observable<float> OnTick => _tickSubject;

        private readonly GameplayConfig _config;
        private readonly GameplayTime _time;
        private readonly Subject<float> _tickSubject;
        private readonly CompositeDisposable _disposables;
        
        public GameplayCore(GameplayConfig config, GameplayTime time)
        {
            _config = config;
            _time = time;
            _tickSubject = new();
            _disposables = new();
        }
        
        public void Initialize()
        {
            Observable.Interval(TimeSpan.FromSeconds(_config.TickDelta), UnityTimeProvider.Update)
                .Subscribe(_ => _tickSubject.OnNext(_config.TickDelta))
                .AddTo(_disposables);
            _time.Start(this);
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _time.Stop();
        }
    }
}