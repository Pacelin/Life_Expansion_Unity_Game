using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings Toolbar Config Installer", fileName = "Buildings Toolbar Config")]
    public class BuildingsToolbarConfigInstaller : JamInstaller
    {
        [SerializeField] private BuildingsToolbarConfig _config;
        protected override void Install()
        {
            Container.Bind<BuildingsToolbarConfig>()
                .FromInstance(_config)
                .AsSingle();
        }
    }
}