using UnityEngine;
using Zenject;

namespace Jamcelin.Runtime.Core
{
    public abstract class JamInstaller : ScriptableObjectInstaller
    {
        public bool Enabled => _enabled;
        [SerializeField] private bool _enabled = true;

        public override void InstallBindings() => Install();
        protected abstract void Install();
    }
}