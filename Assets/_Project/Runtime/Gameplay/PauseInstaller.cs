using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Pause Installer", fileName = "Pause")]
public class PauseInstaller : JamInstaller
{
    [SerializeField]
    private PauseHandler _prefab;
    
    protected override void Install()
    {
        Container.Bind<PauseHandler>()
                 .FromComponentInNewPrefab(_prefab)
                 .AsCanvasView().NonLazy();
    }
}
