using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Misc
{
    public static class ZenjectExtensions
    {
        public static ConcreteIdArgConditionCopyNonLazyBinder AsCanvasView(this NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder binder)
        {
            return binder
                .UnderTransform(ic => ic.Container.Resolve<Canvas>().transform)
                .AsSingle();
        }
    }
}