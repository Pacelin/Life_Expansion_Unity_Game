using UnityEngine;
using UnityEngine.Localization;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Building Bubble Config")]
    public class BuildingBubbleConfig : ScriptableObject
    {
        public string FloodText => _floodString.GetLocalizedString();
        public string NoWaterText => _noWaterString.GetLocalizedString();
        public string CollectText => _collectString.GetLocalizedString();

        public string NoEnergyText => _noEnergy.GetLocalizedString();
        public string NoColonizersText => _noColonizers.GetLocalizedString();
        
        [SerializeField] private LocalizedString _floodString;
        [SerializeField] private LocalizedString _noWaterString;
        [SerializeField] private LocalizedString _collectString;
        [Space]
        [SerializeField] private LocalizedString _noEnergy;
        [SerializeField] private LocalizedString _noColonizers;
    }
}