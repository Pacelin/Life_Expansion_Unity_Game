using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    [CreateAssetMenu(menuName = "Gameplay/Colonizer Config")]
    public class ColonizerConfig : ScriptableObject
    {
        public ColonizerCapsuleConfig Capsule => _capsule;

        public int IdealPopulation => _idealPopulation;
        public int TargetPopulation => _targetPopulation;
        public int AchievementPopulation => _achievementPopulation;
        public float PopulationInterpolation => _populationInterpolation;

        public ColonizerIndicatorConfig Temperature => _temperature;
        public ColonizerIndicatorConfig Oxygen => _oxygen;
        public ColonizerIndicatorConfig Water => _water;
        
        [SerializeField] private ColonizerCapsuleConfig _capsule;
        [Space] 
        [SerializeField] private int _idealPopulation;
        [SerializeField] private int _targetPopulation;
        [SerializeField] private int _achievementPopulation;
        [SerializeField] private float _populationInterpolation = 0.5f;
        [Space]
        [SerializeField] private ColonizerIndicatorConfig _temperature;
        [SerializeField] private ColonizerIndicatorConfig _oxygen;
        [SerializeField] private ColonizerIndicatorConfig _water;
    }
}