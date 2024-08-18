using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Toolbar Config")]
    public class BuildingsToolbarConfig : ScriptableObject
    {
        public IReadOnlyList<BuildingConditionalConfig> AvailableBuildings => _availableBuildings;
        [SerializeField] private BuildingConditionalConfig[] _availableBuildings;
    }
}