using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Core
{
    [CreateAssetMenu(menuName = "Gameplay/Gameplay Project Installer", fileName = "Gameplay Project")]
    public class GameplayProjectInstaller : JamInstaller
    {
        [SerializeField] private GameplayConfig _config;
        protected override void Install()
        {
            Container.Bind<GameplayConfig>()
                .FromInstance(_config)
                .AsSingle();
            Container.Bind<GameplayTime>()
                .AsSingle();
        }
    }
}