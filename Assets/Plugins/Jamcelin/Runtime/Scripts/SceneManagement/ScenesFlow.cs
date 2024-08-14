using System;
using Cysharp.Threading.Tasks;
using Jamcelin.Runtime.Scoping;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;

namespace Jamcelin.Runtime.SceneManagement
{
    public class ScenesFlow
    {
        private readonly SceneReferences _references;
        private JamScope _sceneScope;
        
        public ScenesFlow(SceneReferences references)
        {
            _references = references;
        }

        public async UniTask SwitchScene(EScene scene, Action<IContainerBuilder> extraArgs = null)
        {
            await Addressables.LoadSceneAsync(_references.EmptyScene);
            
            var reference = _references.Dictionary[scene];
            var scopeHandle = Addressables.InstantiateAsync(reference.Scope);
            var scopeGo = await scopeHandle;
            var scope = scopeGo.GetComponent<JamScope>();
            JamScope.Create();
            var reference.Scope.LoadAssetAsync();
            
            await Addressables.LoadSceneAsync()
        }
    }
}