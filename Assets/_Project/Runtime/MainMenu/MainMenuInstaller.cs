using Jamcelin.Runtime.Core;
using UnityEngine;

public class MainMenuInstaller : JamInstaller
{
    protected override void Install()
    {
    {
        Container.Bind<MainMenuUiView>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}
}
