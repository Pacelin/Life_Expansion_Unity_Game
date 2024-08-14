using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Jamcelin.Runtime
{
    public abstract class JamInstaller : ScriptableObject, IInstaller
    {
        public bool Enabled => _enabled;
        
        [SerializeField] private bool _enabled = true;
        
        public abstract void Install(IContainerBuilder builder);
    }
}