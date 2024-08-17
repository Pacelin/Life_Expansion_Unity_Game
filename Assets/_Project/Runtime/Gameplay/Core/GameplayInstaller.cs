using Jamcelin.Runtime.Core;
using UnityEngine;

namespace Runtime.Gameplay.Core
{
    [CreateAssetMenu(menuName = "Gameplay/Gameplay Installer", fileName = "Gameplay")]
    public class GameplayInstaller : JamInstaller
    {
        protected override void Install()
        {
            Container.BindInterfacesAndSelfTo<GameplayCore>()
                .AsSingle();
        }
    }
}