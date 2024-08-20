using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.General
{
    [CreateAssetMenu(menuName = "Runtime/Building Interactor Installer", fileName = "Building Interactor")]
    public class BuildingInteractorInstaller : JamInstaller
    {
        [SerializeField] private BuildingInteractorView _viewPrefab;
        [SerializeField] private LayerMask _interactMask;
        
        protected override void Install()
        {
            Container.Bind<BuildingInteractorView>()
                .FromComponentInNewPrefab(_viewPrefab)
                .AsCanvasView()
                .OnInstantiated<BuildingInteractorView>((ic, o) => o.gameObject.SetActive(false));
            Container.BindInterfacesAndSelfTo<BuildingInteractor>()
                .AsSingle()
                .WithArguments((int)_interactMask);
        }
    }
}