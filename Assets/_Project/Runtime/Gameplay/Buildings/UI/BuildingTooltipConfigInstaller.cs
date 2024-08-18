using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.UI
{
    [CreateAssetMenu(menuName = "Gameplay/Building Tooltip Config Installer")]
    public class BuildingTooltipConfigInstaller : JamInstaller
    {
        [SerializeField] private BuildingTooltipConfig _config;
        protected override void Install()
        {
            Container.Bind<BuildingTooltipConfig>()
                .FromInstance(_config)
                .AsSingle();
        }
    }
}