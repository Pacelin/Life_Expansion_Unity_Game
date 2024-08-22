using Runtime.Gameplay.Buildings;
using Runtime.Gameplay.Colonizers;
using Runtime.Gameplay.Planets;
using UnityEngine;
using UnityEngine.Localization;

namespace Runtime.MainMenu
{
    [System.Serializable]
    public class LevelStartConfig
    {
        public ColonizersConfig Colonizers;
        public PlanetConfig Planet;
        public BuildingsToolbarConfig Toolbar;
        public Sprite PlanetIcon;
        public LocalizedString PlanetName => _planetName;

        [SerializeField] private LocalizedString _planetName;
    }
}