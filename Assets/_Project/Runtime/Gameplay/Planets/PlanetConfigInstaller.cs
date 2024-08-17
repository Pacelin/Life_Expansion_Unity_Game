using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Planets
{
    [CreateAssetMenu(menuName = "Gameplay/Planet Config Installer", fileName = "Planet Config")]
    public class PlanetConfigInstaller : JamInstaller
    {
        [SerializeField] private PlanetConfig _config;
        protected override void Install()
        {
            Container.Bind<PlanetConfig>()
                .FromInstance(_config)
                .AsSingle();
        }
    }
}