using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/Level Installer", fileName = "Level")]
    public class LevelInstaller : JamInstaller
    {
        [SerializeField] private LevelConfig _config;

        protected override void Install()
        {
            Container.Bind<LevelConfig>()
                .FromInstance(_config)
                .AsSingle();
        }
    }
}