using System;
using R3;
using UnityEngine;

namespace Runtime.Gameplay.Core
{
    public class GameplayTime
    {
        public ReadOnlyReactiveProperty<DateTime> Value => _value;
        
        private readonly ReactiveProperty<DateTime> _value;
        private readonly float _timeScale;

        private IDisposable _timeDisposable;
        
        public GameplayTime(GameplayConfig config)
        {
            _value = new(new DateTime(config.InitialYear, 1, 1));
            _timeScale = config.TimeScale;
        }

        public void Start(GameplayCore core)
        {
            _timeDisposable = core.OnTick
                .Subscribe(delta => _value.Value += TimeSpan.FromSeconds(delta * _timeScale));
        }

        public void Stop()
        {
            _timeDisposable.Dispose();
        }
    }
}