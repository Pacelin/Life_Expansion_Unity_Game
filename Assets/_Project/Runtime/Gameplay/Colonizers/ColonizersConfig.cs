using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    [CreateAssetMenu(menuName = "Gameplay/Colonizers Config")]
    public class ColonizersConfig : ScriptableObject
    {
        public ColonizersCapsuleConfig Capsule => _capsule;

        public int IdealPopulation => _idealPopulation;
        public int TargetPopulation => _targetPopulation;
        public int AchievementPopulation => _achievementPopulation;
        public float PopulationInterpolation => _populationInterpolation;

        public ColonizersIndicatorConfig Temperature => _temperature;
        public ColonizersIndicatorConfig Oxygen => _oxygen;
        public ColonizersIndicatorConfig Water => _water;
        
        [SerializeField] private ColonizersCapsuleConfig _capsule;
        [Space] 
        [SerializeField] private int _idealPopulation;
        [SerializeField] private int _targetPopulation;
        [SerializeField] private int _achievementPopulation;
        [SerializeField] private float _populationInterpolation = 0.5f;
        [Space]
        [SerializeField] private ColonizersIndicatorConfig _temperature;
        [SerializeField] private ColonizersIndicatorConfig _oxygen;
        [SerializeField] private ColonizersIndicatorConfig _water;
    }
}