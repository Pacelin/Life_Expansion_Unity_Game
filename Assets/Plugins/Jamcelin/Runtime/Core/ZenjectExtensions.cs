using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace Jamcelin.Runtime.Core
{
    public static class ZenjectExtensions
    {
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder FromScriptableObjectAddressable<T>(
            this FromBinderGeneric<T> binder, string key)
        {
            return binder.FromMethod(ic =>
            {
                var handle = Addressables.LoadAssetAsync<T>(key);
                var manager = ic.Container.Resolve<DisposableManager>();
                manager.Add(Disposable.Create(() => Addressables.Release(handle)));
                return handle.WaitForCompletion();
            });
        }
        
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewScriptableObjectAddressable<T>(
            this FromBinderGeneric<T> binder, string key) where T : ScriptableObject
        {
            return binder.FromMethod(_ =>
            {
                var handle = Addressables.LoadAssetAsync<T>(key);
                var asset = handle.WaitForCompletion();
                var newAsset = Object.Instantiate(asset);
                Addressables.Release(handle);
                return newAsset;
            });
        }

        public static ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefabAddressable<T>(
            this FromBinderGeneric<T> binder, string key) where T : Component
        {
            return binder.FromMethod(_ =>
            {
                var handle = Addressables.InstantiateAsync(key);
                var go = handle.WaitForCompletion();
                return go.GetComponent<T>();
            });
        }

        public static IList<JamInstaller> Inject(this DiContainer container, AssetLabelReference scope, 
            out AsyncOperationHandle<IList<JamInstaller>>? handle)
        {
            handle = null;
            var resourceLocationsHandle =
                Addressables.LoadResourceLocationsAsync(scope.labelString, typeof(JamInstaller));
            var resourceLocations = resourceLocationsHandle.WaitForCompletion();
            if (resourceLocations.Count <= 0)
                return new List<JamInstaller>();

            handle = Addressables.LoadAssetsAsync<JamInstaller>(resourceLocations, _ => { });
            var installers = handle.Value.WaitForCompletion();
            foreach (var installer in installers)
                if (installer.Enabled)
                    container.Inject(installer);
            return installers;
        }

        public static void Install(this IList<JamInstaller> installers,
            AsyncOperationHandle<IList<JamInstaller>>? handle)
        {
            if (handle == null)
                return;
            foreach (var installer in installers)
                if (installer.Enabled)
                    installer.InstallBindings();
            Addressables.Release(handle.Value);
        }
    }
}