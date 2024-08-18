using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;

namespace Runtime.Gameplay.Planets.UI
{
    [CreateAssetMenu(menuName = "UI/Planet Info Installer", fileName = "Planet Info UI")]
    public class PlanetInfoUIInstaller : JamInstaller
    {
        [SerializeField] private PlanetInfoView _viewPrefab;
        
        protected override void Install()
        {
            Container.Bind<PlanetInfoView>()
                .FromComponentInNewPrefab(_viewPrefab)
                .AsCanvasView();
            Container.BindInterfacesAndSelfTo<PlanetInfoPresenter>()
                .AsSingle();
        }
    }
}