using R3;
using Runtime.Gameplay.Planets;
using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    public class ColonizersIndicatorModel
    {
        public ReadOnlyReactiveProperty<float> Feeling => _felling;
        private readonly ReadOnlyReactiveProperty<float> _felling;

        public ColonizersIndicatorModel(ColonizersIndicatorConfig config, 
            PlanetIndicator planetIndicator)
        {
            _felling = planetIndicator.Value.Select(v =>
            {
                return Mathf.Clamp01(1 - (v - config.Target) / 
                    ((v < config.Target ? config.Min : config.Max) - config.Target));
            }).ToReadOnlyReactiveProperty();
        }
    }
}