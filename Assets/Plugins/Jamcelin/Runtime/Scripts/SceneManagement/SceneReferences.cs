using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jamcelin.Runtime.SceneManagement
{
    [CreateAssetMenu(fileName = "Scene References", menuName = "Jamcelin/Scene References", order = 0)]
    public class SceneReferences : ScriptableObject
    {
        public IReadOnlyDictionary<EScene, SceneReference> Dictionary => _dictionary;
        public AssetReference BootScene => _bootScene;
        public AssetReference EmptyScene => _emptyScene;
        
        [SerializedDictionary("Scene", "Reference")]
        [SerializeField] private SerializedDictionary<EScene, SceneReference> _dictionary;
        [SerializeField] private AssetReference _bootScene;
        [SerializeField] private AssetReference _emptyScene;
    }
}