using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    [CreateAssetMenu(menuName = "Gameplay/Colonizers Config Installer", fileName = "Colonizers Config")]
    public class ColonizersConfigInstaller : JamInstaller
    {
        [SerializeField] private ColonizersConfig _config;
        protected override void Install()
        {
            Container.Bind<ColonizersConfig>()
                .FromInstance(_config)
                .AsSingle();
        }
    }
}