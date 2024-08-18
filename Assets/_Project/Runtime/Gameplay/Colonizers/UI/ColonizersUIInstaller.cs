using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;

namespace Runtime.Gameplay.Colonizers.UI
{
    [CreateAssetMenu(menuName = "Gameplay/Colonizers UI Installer", fileName = "Colonizers UI")]
    public class ColonizersUIInstaller : JamInstaller
    {
        [SerializeField] private ColonizersInfoView _prefab;
        
        protected override void Install()
        {
            Container.Bind<ColonizersInfoView>()
                .FromComponentInNewPrefab(_prefab)
                .AsCanvasView();
            Container.BindInterfacesAndSelfTo<ColonizersInfoPresenter>()
                .AsSingle();
        }
    }
}