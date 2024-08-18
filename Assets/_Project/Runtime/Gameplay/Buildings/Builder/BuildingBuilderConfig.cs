using UnityEngine;

namespace Runtime.Gameplay.Buildings.Builder
{
    [CreateAssetMenu(menuName = "Gameplay/Building Builder Config", fileName = "Building Builder Config")]
    public class BuildingBuilderConfig : ScriptableObject
    {
        public LayerMask BuildLayer => _buildLayer;
        public LayerMask PlanetLayer => _planetLayer;
        
        [SerializeField] private LayerMask _buildLayer;
        [SerializeField] private LayerMask _planetLayer;
    }
}