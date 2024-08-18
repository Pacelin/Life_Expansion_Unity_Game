using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    [CreateAssetMenu(menuName = "Gameplay/Colonizers Installer", fileName = "Colonizers")]
    public class ColonizersInstaller : JamInstaller
    {
        protected override void Install()
        {
            Container.Bind<ColonizersModel>()
                .AsSingle();
        }
    }
}