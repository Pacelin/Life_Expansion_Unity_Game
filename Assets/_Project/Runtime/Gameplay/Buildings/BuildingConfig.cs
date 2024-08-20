using Runtime.Gameplay.Buildings.UI;
using UnityEngine;
using UnityEngine.Localization;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Building Config")]
    public class BuildingConfig : ScriptableObject
    {
        public BuildingView Prefab => _prefab;

        public int MineralsCost => _mineralsCost;
        public int ColonizersCost => _colonizersCost;
        public int EnergyCost => _energyCost;
        public EBuildTerritory BuildTerritory => _buildTerritory;
        
        public string Name => _name.GetLocalizedString();
        public string Description => _description.GetLocalizedString();
        public Sprite Icon => _icon;
        public BuildingsToolbarTabConfig ToolbarTab => _toolbarTab;
        
        [SerializeField] private BuildingView _prefab;
        [SerializeField] private int _mineralsCost;
        [SerializeField] private int _colonizersCost;
        [SerializeField] private int _energyCost;
        [SerializeField] private EBuildTerritory _buildTerritory = EBuildTerritory.Ground;
        [Space] 
        [SerializeField] private LocalizedString _name;
        [SerializeField] private LocalizedString _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private BuildingsToolbarTabConfig _toolbarTab;
    }
}