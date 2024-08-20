using R3;
using Runtime.Gameplay.Planets;
using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    public class ColonizersIndicatorModel
    {
        public ReadOnlyReactiveProperty<float> Feeling => _feeling;
        public float TargetFeeling => _targetFeeling;
        
        private readonly ReadOnlyReactiveProperty<float> _feeling;
        private readonly float _targetFeeling;

        public ColonizersIndicatorModel(ColonizersIndicatorConfig config, 
            PlanetIndicator planetIndicator)
        {
            _targetFeeling = 1 - (config.Max - config.Target) / (config.Max - config.Min);
            _feeling = planetIndicator.Value.Select(v =>
            {
                return Mathf.Clamp01(1 - (v - config.Target) / 
                    ((v < config.Target ? config.Min : config.Max) - config.Target));
            }).ToReadOnlyReactiveProperty();
        }
    }
}