using Jamcelin.Runtime.Scoping;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jamcelin.Runtime
{
    public static class EntryPoint
    {
        private const string SETTINGS_KEY = "Entry Point Settings";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            var settingsHandle = Addressables.LoadAssetAsync<EntryPointSettings>(SETTINGS_KEY);
            var settings = settingsHandle.WaitForCompletion();

            var scopeGoHandle = Addressables.InstantiateAsync(settings.ProjectScopeReference);
            var scopeGo = scopeGoHandle.WaitForCompletion();
            Object.DontDestroyOnLoad(scopeGo);
            
            var sceneHandle = Addressables.LoadSceneAsync(settings.FirstScene);
            sceneHandle.WaitForCompletion();
            
            Addressables.Release(settingsHandle);
        }
    }
}