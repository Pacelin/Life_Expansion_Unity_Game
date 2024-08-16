using Jamcelin.Runtime.Core;

namespace Jamcelin.Runtime.Entities
{
    public class EntityContext : JamGameObjectContext
    {
        protected override void InstallInstallers()
        {
            base.InstallInstallers();
            Container.Bind<EntityKernel>()
                .FromNewComponentOnRoot()
                .AsSingle()
                .WhenInjectedInto<Entity>();
            Container.Bind<Entity>()
                .AsSingle();
        }
    }
}