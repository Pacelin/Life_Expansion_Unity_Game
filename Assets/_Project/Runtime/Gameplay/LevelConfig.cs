using UnityEngine;

namespace Runtime.Gameplay
{
    [System.Serializable]
    public class LevelConfig
    {
        public Planet PlanetPrefab => _planetPrefab;
        
        [SerializeField] private Planet _planetPrefab;
    }
}