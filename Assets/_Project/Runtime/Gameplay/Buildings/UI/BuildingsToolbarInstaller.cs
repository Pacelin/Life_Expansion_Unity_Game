using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.UI
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings Toolbar Installer", fileName = "Buildings Toolbar")]
    public class BuildingsToolbarInstaller : JamInstaller
    {
        [SerializeField] private BuildingsToolbarView _prefab;
        [Inject] private BuildingsToolbarConfig _config;
        
        protected override void Install()
        {
            Container.Bind<BuildingsToolbarView>()
                .FromComponentInNewPrefab(_prefab)
                .AsCanvasView();
            Container.BindInterfacesAndSelfTo<BuildingsToolbarPresenter>()
                .AsSingle()
                .OnInstantiated<BuildingsToolbarPresenter>((ic, o) =>
                {
                    foreach (var building in _config.AvailableBuildings)
                        o.Add(building);
                });
        }
    }
}