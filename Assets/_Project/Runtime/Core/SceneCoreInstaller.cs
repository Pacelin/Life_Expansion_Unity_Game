using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Core
{
    [CreateAssetMenu(menuName = "Runtime/Scene Core Installer", fileName = "Scene Core")]
    public class SceneCoreInstaller : JamInstaller
    {
        protected override void Install()
        {
            Container.Bind<Camera>()
                .FromInstance(Camera.main)
                .AsSingle();
        }
    }
}