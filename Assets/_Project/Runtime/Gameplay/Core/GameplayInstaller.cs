using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;

namespace Runtime.Gameplay.Core
{
    [CreateAssetMenu(menuName = "Gameplay/Gameplay Installer", fileName = "Gameplay")]
    public class GameplayInstaller : JamInstaller
    {
        [SerializeField] private GameplayTimeView _gameplayTimePrefab;
        
        protected override void Install()
        {
            Container.BindInterfacesAndSelfTo<GameplayCore>()
                .AsSingle();
            Container.Bind<GameplayTimeView>()
                .FromComponentInNewPrefab(_gameplayTimePrefab)
                .AsCanvasView();
            Container.BindInterfacesAndSelfTo<GameplayTimePresenter>()
                .AsSingle();
        }
    }
}