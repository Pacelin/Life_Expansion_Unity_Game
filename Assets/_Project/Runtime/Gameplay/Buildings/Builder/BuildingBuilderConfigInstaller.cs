using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.Builder
{
    [CreateAssetMenu(menuName = "Gameplay/Building Builder Config Installer", fileName = "Building Builder Config")]
    public class BuildingBuilderConfigInstaller : JamInstaller
    {
        [SerializeField] private BuildingBuilderConfig _config;
        protected override void Install()
        {
            Container.Bind<BuildingBuilderConfig>()
                .FromInstance(_config)
                .AsSingle();
        }
    }
}