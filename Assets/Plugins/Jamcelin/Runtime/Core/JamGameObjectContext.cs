using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Jamcelin.Runtime.Core
{
    public class JamGameObjectContext : GameObjectContext
    {
        [SerializeField] private AssetLabelReference _scope;

        protected override void InstallInstallers()
        {
            base.InstallInstallers();
            var installer = Container.Instantiate<ScopeInstaller>();
            installer.InstallScope(_scope);
            installer.InstallBindings();
        }
    }
}