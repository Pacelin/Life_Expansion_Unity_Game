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
            _currentPopulation.Select(v => v >= _config.TargetPopulation)
                .ToReadOnlyReactiveProperty();

        public ReadOnlyReactiveProperty<bool> IsAchievementCompleted => 
            _currentPopulation.Select(v => v >= _config.AchievementPopulation)
                .ToReadOnlyReactiveProperty();

        private readonly ColonizersConfig _config;
        private readonly ReactiveProperty<float> _currentPopulation;
        private readonly ReactiveProperty<int> _maxPopulation;

        public ColonizersPopulationModel(
            ColonizersConfig config)
        {
            _config = config;
            _currentPopulation = new(_config.Capsule.InitialPopulation);
            _maxPopulation = new(_config.Capsule.InitialPopulation);
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
    }
}