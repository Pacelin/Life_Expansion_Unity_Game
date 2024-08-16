using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Jamcelin.Runtime.Core
{
    public class ScopeInstaller : InstallerBase
    {
        private AsyncOperationHandle<IList<JamInstaller>>? _handle;
        private IList<JamInstaller> _installers;

        public void InstallScope(AssetLabelReference scope)
        {
            _installers = Container.Inject(scope, out _handle);
        }

        public override void InstallBindings()
        {
            _installers.Install(_handle);
            _installers = null;
            _handle = null;
        }
    }
}