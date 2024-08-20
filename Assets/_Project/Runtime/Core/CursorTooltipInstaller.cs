using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;

namespace Runtime.Core
{
    [CreateAssetMenu(menuName = "Runtime/Cursor Tooltip Installer", fileName = "Cursor Tooltip")]
    public class CursorTooltipInstaller : JamInstaller
    {
        [SerializeField] private CursorTooltipView _viewPrefab;
        protected override void Install()
        {
            Container.Bind<CursorTooltipView>()
                .FromComponentInNewPrefab(_viewPrefab)
                .AsCanvasView();
            Container.BindInterfacesAndSelfTo<CursorTooltip>()
                .AsSingle();
        }
    }
}