using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Core
{
    [CreateAssetMenu(menuName = "Runtime/Scene Camera Installer", fileName = "Scene Camera")]
    public class SceneCameraInstaller : JamInstaller
    {
        protected override void Install()
        {
            Container.Bind<Camera>()
                .FromInstance(Camera.main)
                .AsSingle();
        }
    }
}