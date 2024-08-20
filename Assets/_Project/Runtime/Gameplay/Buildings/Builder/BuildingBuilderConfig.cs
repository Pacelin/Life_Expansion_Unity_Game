using UnityEngine;

namespace Runtime.Gameplay.Buildings.Builder
{
    [CreateAssetMenu(menuName = "Gameplay/Building Builder Config", fileName = "Building Builder Config")]
    public class BuildingBuilderConfig : ScriptableObject
    {
        public LayerMask BuildLayer => _buildLayer;
        public LayerMask PlanetLayer => _planetLayer;
        public string WaterTag => _waterTag;

        public AnimationCurve BuildAnimationCurve => _buildAnimationCurve;
        public float BuildAnimationDuration => _buildAnimationDuration;
        public float BuildAnimationStartDistance => _buildAnimationStartDistance;
        
        [SerializeField] private LayerMask _buildLayer;
        [SerializeField] private LayerMask _planetLayer;
        [SerializeField] private string _waterTag;
        [Space]
        [SerializeField] private AnimationCurve _buildAnimationCurve;
        [SerializeField] private float _buildAnimationDuration;
        [SerializeField] private float _buildAnimationStartDistance;
    }
}