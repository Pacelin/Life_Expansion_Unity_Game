using Jamcelin.Runtime.Core;
using Runtime.Gameplay.Misc;
using UnityEngine;

namespace Runtime.MainMenu
{
    [CreateAssetMenu]
    public class MainMenuInstaller : JamInstaller
    {
        [SerializeField] private MainMenuView _viewPrefab;
        [SerializeField] private SettingsView _settingsViewPrefab;
        [SerializeField] private LevelSelectionView _selectionViewPrefab;
        [Space] 
        [SerializeField] private LevelStartConfig[] _levels;
        
        protected override void Install()
        {
            Container.Bind<SettingsView>()
                .FromComponentInNewPrefab(_settingsViewPrefab)
                .AsCanvasView()
                .OnInstantiated<SettingsView>((ic, o) => o.gameObject.SetActive(false));
            Container.BindInterfacesAndSelfTo<SettingsPresenter>()
                .AsSingle();

            Container.Bind<LevelSelectionView>()
                .FromComponentInNewPrefab(_selectionViewPrefab)
                .AsCanvasView()
                .OnInstantiated<LevelSelectionView>((ic, o) => o.gameObject.SetActive(false));
            Container.BindInterfacesAndSelfTo<LevelSelectionPresenter>()
                .AsSingle();
            Container.Bind<LevelStartConfig[]>()
                .FromInstance(_levels)
                .AsSingle()
                .WhenInjectedInto<LevelSelectionPresenter>();

            Container.Bind<MainMenuView>()
                .FromComponentInNewPrefab(_viewPrefab)
                .AsCanvasView()
                .OnInstantiated<MainMenuView>((ic, o) => o.gameObject.SetActive(true));
            Container.BindInterfacesAndSelfTo<MainMenuPresenter>()
                .AsSingle();
        }
    }
}