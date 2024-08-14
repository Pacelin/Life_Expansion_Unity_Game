using Jamcelin.Runtime.Scoping;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jamcelin.Runtime.SceneManagement
{
    [System.Serializable]
    public class SceneReference
    {
        public AssetReference Scene => _scene;
        public AssetReferenceT<JamScope> Scope => _scope;
        
        [SerializeField] private AssetReference _scene;
        [SerializeField] private AssetReferenceT<JamScope> _scope;
    }
}