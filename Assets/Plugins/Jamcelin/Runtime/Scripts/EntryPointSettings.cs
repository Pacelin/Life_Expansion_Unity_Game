using Jamcelin.Runtime.Scoping;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jamcelin.Runtime
{
    public class EntryPointSettings : ScriptableObject
    {
        public AssetReference FirstScene => _firstScene;
        public AssetReferenceT<JamScope> ProjectScopeReference => _projectScopeReference;
        
        [SerializeField] private AssetReference _firstScene;
        [SerializeField] private AssetReferenceT<JamScope> _projectScopeReference;
    }
}