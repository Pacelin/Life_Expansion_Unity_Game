using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Buildings.General;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.Builder
{
    [CreateAssetMenu(menuName = "Gameplay/Building Builder Installer", fileName = "Building Builder")]
    public class BuildingBuilderInstaller : JamInstaller
    {
        [SerializeField] private BuildingBuilderView _builderPrefab;
        
        protected override void Install()
        {
            Container.Bind<BuildingBuilderView>()
                .FromComponentInNewPrefab(_builderPrefab)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<BuildingApplier>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<BuildingFactory>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<BuildingBuilder>()
                .AsSingle();
            Container.Bind<BuildService>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<BuildingsValidator>()
                .AsSingle();
        }
    }
}