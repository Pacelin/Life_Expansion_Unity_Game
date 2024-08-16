using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Jamcelin.Runtime.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jamcelin.Runtime.Extras.SceneManagement
{
    [CreateAssetMenu(menuName = "Jamcelin/Scene Manager Installer")]
    public class SceneManagerInstaller : JamInstaller
    {
        [SerializeField] private SerializedDictionary<EScene, AssetReference> _scenes;
        
        protected override void Install()
        {
            Container.Bind<SceneManager>()
                .AsSingle()
                .WithArguments((IReadOnlyDictionary<EScene, AssetReference>) _scenes);
        }
    }
}