using System;
using R3;
using Runtime.Gameplay.Core;
using UnityEngine;

namespace Runtime.Gameplay.Planets
{
    public class PlanetIndicator : IDisposable
    {
        public ReadOnlyReactiveProperty<float> Value => _value;
        public ReadOnlyReactiveProperty<float> NormalizedValue => _value.Select(v =>
            Mathf.Clamp01((_config.Max - _config.Min) / (v - _config.Min))).ToReadOnlyReactiveProperty();
        
        private float _targetValue;
        
        private readonly PlanetIndicatorConfig _config;
        private readonly ReactiveProperty<float> _value;
        private readonly CompositeDisposable _disposables;
        
        public PlanetIndicator(PlanetIndicatorConfig config)
        {
            _config = config;
            _value = new(_config.Initial);
            _disposables = new();
            
            _targetValue = _config.Initial;
        }

        public void ApplyDelta(float value)
        {
            if (value == 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _targetValue += value * _config.Sensitivity;
        }

        public void Initialize(GameplayCore gameplay)
        {
            gameplay.OnTick.Subscribe(Tick)
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();

        private void Tick(float delta)
        {
            
        }
    }
}