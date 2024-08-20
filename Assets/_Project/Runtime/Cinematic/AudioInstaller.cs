using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Cinematic
{
    [CreateAssetMenu(menuName = "Runtime/Audio Installer", fileName = "Audio")]
    public class AudioInstaller : JamInstaller
    {
        [SerializeField] private AudioComponent _componentPrefab;
        [SerializeField] private AudioDatabase _database;
        
        protected override void Install()
        {
            Container.Bind<AudioComponent>()
                .FromComponentInNewPrefab(_componentPrefab)
                .AsSingle();
            Container.Bind<AudioDatabase>()
                .FromInstance(_database)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<Audio>()
                .AsSingle()
                .NonLazy();
        }
    }
}