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
            _targetFeeling = (config.Target - config.Min) / (config.Max - config.Min);
            _feeling = planetIndicator.Value.Select(v =>
            {
                if (v < config.Target)
                    return Mathf.Clamp01((v - config.Min) / (config.Target - config.Min));
                return 1 - Mathf.Clamp01((v - config.Target) / (config.Max - config.Target));
            }).ToReadOnlyReactiveProperty();
        }
    }
}