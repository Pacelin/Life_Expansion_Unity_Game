using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Jamcelin.Runtime.Core
{
    public class JamSceneContext : SceneContext
    {
        [SerializeField] private AssetLabelReference _scope;

        protected override void InstallInstallers()
        {
            base.InstallInstallers();
            var installers = 
                Container.Inject(_scope, out var handle);
            installers.Install(handle);
        }
    }
}