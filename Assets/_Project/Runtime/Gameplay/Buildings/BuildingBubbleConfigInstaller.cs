using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Building Bubble Config Installer", fileName = "Building Bubble Config")]
    public class BuildingBubbleConfigInstaller : JamInstaller
    {
        [SerializeField] private BuildingBubbleConfig _config;
        
        protected override void Install()
        {
            Container.Bind<BuildingBubbleConfig>()
                .FromInstance(_config)
                .AsSingle();
        }
    }
}