using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Planets
{
    [CreateAssetMenu(menuName = "Gameplay/Planet Installer", fileName = "Planet")]
    public class PlanetInstaller : JamInstaller
    {
        protected override void Install()
        {
            Container.Bind<PlanetComponent>()
                .FromResolveGetter<PlanetConfig>(config =>
                {
                    var solarPrefab = config.SolarSystem;
                    var solarInstance = Instantiate(solarPrefab, Vector3.zero, Quaternion.identity);
                    solarInstance.name = "Solar System";
                    return solarInstance.GetComponentInChildren<PlanetComponent>();

                    //var prefab = config.Prefab;
                    //var obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    //obj.gameObject.name = "Planet";
                    //return obj;
                })
                .AsSingle()
                .WhenInjectedInto<Planet>();
            Container.BindInterfacesAndSelfTo<Planet>()
                .AsSingle();
            Container.BindInterfacesTo<PlanetSync>()
                .AsSingle();
        }
    }
}