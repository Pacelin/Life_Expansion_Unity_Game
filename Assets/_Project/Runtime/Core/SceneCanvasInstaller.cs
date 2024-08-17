using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Core
{
    [CreateAssetMenu(menuName = "Runtime/Scene Canvas Installer", fileName = "Scene Canvas")]
    public class SceneCanvasInstaller : JamInstaller
    {
        protected override void Install()
        {
            Container.Bind<Canvas>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}