using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Jamcelin.Runtime.SceneManagement
{
    public class SceneManager
    {
        private readonly IReadOnlyDictionary<EScene, AssetReference> _scenes;

        public SceneManager(IReadOnlyDictionary<EScene, AssetReference> scenes) =>
            _scenes = scenes;

        public async UniTask SwitchScene(EScene scene) =>
            await Addressables.LoadSceneAsync(_scenes[scene]);

        public async UniTask AddScene(EScene scene) =>
            await Addressables.LoadSceneAsync(_scenes[scene], LoadSceneMode.Additive);
    }
}