using NaughtyAttributes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Jamcelin.Runtime.Scoping
{
    public class JamScope : LifetimeScope
    {
        [ReadOnly] [SerializeField] private JamInstaller[] _installers;

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (var installer in _installers)
                installer.Install(builder);
        }
    }
}