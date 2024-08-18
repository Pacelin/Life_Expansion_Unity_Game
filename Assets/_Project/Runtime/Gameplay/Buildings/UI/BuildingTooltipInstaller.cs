using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.UI
{
    [CreateAssetMenu(menuName = "Gameplay/Building Tooltip Installer")]
    public class BuildingTooltipInstaller : JamInstaller
    {
        [SerializeField] private BuildingTooltipView _prefab;
        
        protected override void Install()
        {
            Container.Bind<BuildingTooltipView>()
                .FromComponentInNewPrefab(_prefab)
                .AsCanvasView();
            Container.BindInterfacesAndSelfTo<BuildingTooltipPresenter>()
                .AsSingle();
        }
    }
}