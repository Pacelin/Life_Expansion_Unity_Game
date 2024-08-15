using Jamcelin.Runtime;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Runtime
{
    [CreateAssetMenu(menuName = "Installers/Test")]
    public class TestInstaller : JamInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            Debug.Log("Test Installer");
            builder.Register<Test>(Lifetime.Scoped)
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }

    public class Test : IStartable
    {
        public void Start()
        {
            Debug.Log("Start");
        }
    }
}