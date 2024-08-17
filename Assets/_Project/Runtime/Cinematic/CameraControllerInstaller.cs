using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Cinematic
{
    [CreateAssetMenu(menuName = "Cinematic/Camera Controller Installer", fileName = "Camera Controller")]
    public class CameraControllerInstaller : JamInstaller
    {
        [SerializeField] private CameraControllerConfig _config;
        protected override void Install()
        {
            Container.Bind<CameraControllerConfig>()
                .FromInstance(_config)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<CameraController>()
                .AsSingle()
                .NonLazy();
        }
    }
}