using Jamcelin.Runtime.Core;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/Planet Installer", fileName = "Planet")]
    public class PlanetInstaller : JamInstaller
    {
        [Inject] private LevelConfig _level;
        
        protected override void Install()
        {
            Container.Bind<Planet>()
                .FromComponentInNewPrefab(_level.PlanetPrefab)
                .WithGameObjectName("Planet")
                .AsSingle()
                .OnInstantiated<Planet>((ic, o) => o.transform.position = Vector3.zero);
        }
    }
}