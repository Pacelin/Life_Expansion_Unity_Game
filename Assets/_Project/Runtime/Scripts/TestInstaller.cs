using Jamcelin.Runtime.Core;
using UnityEngine;
using Zenject;

namespace Runtime
{
    [CreateAssetMenu(menuName = "Installers/Test Installer")]
    public class TestInstaller : JamInstaller
    {
        protected override void Install()
        {
            Debug.Log("Test Install");
            Container.BindInterfacesAndSelfTo<Test>()
                .AsSingle();
        }
    }

    public class Test : IInitializable
    {
        public void Initialize()
        {
            Debug.Log("Test Initialize");
        }
    }
}