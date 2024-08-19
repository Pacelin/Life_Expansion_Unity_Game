using R3;
using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    public class ColonizersPopulationModel
    {
        public int Ideal => _config.IdealPopulation;
        public int Target => _config.TargetPopulation;
        public int Achievement => _config.AchievementPopulation;
        public float Interpolation => _config.PopulationInterpolation;
        
        public ReadOnlyReactiveProperty<int> CurrentPopulation => _currentPopulation
            .Select(v => (int) v).ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<int> MaxPopulation => _maxPopulation;

        public ReadOnlyReactiveProperty<bool> IsTargetCompleted => 
            _currentPopulation.Select(v =>
                {
                    var curCompleted = v >= _config.TargetPopulation;
                    if (curCompleted)
                        _targetCompleted = true;
                    return _targetCompleted;
                })
                .ToReadOnlyReactiveProperty();

        public ReadOnlyReactiveProperty<bool> IsAchievementCompleted => 
            _currentPopulation.Select(v => v >= _config.AchievementPopulation)
                .ToReadOnlyReactiveProperty();

        private readonly ColonizersConfig _config;
        private readonly ReactiveProperty<float> _currentPopulation;
        private readonly ReactiveProperty<int> _maxPopulation;
        private readonly ReactiveProperty<int> _busy;

        private bool _targetCompleted;

        public ColonizersPopulationModel(
            ColonizersConfig config)
        {
            _config = config;
            _currentPopulation = new(_config.Capsule.InitialPopulation);
            _maxPopulation = new(_config.Capsule.InitialPopulation);
            _busy = new(0);
            _targetCompleted = false;
        }

        public void Set(float value)
        {
            _currentPopulation.Value = Mathf.Clamp(value, 0, _maxPopulation.Value);
        }
        
        public void ApplyDeltaToMax(int change)
        {
            _maxPopulation.Value += change;
            _currentPopulation.Value = Mathf.Clamp(_currentPopulation.Value, 0, _maxPopulation.Value);
        }

        public void ApplyDeltaToBusy(int change)
        {
            _busy.Value += change;
        }
    }
}