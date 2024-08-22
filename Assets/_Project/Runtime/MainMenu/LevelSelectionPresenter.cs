using System;
using Cysharp.Threading.Tasks;
using Jamcelin.Runtime.SceneManagement;
using R3;
using Runtime.Gameplay.Buildings;
using Runtime.Gameplay.Colonizers;
using Runtime.Gameplay.Planets;
using UnityEngine;
using Zenject;

namespace Runtime.MainMenu
{
    public class LevelSelectionPresenter : IInitializable, IDisposable
    {
        [Inject] private LevelStartConfig[] _levels;
        [Inject] private LevelSelectionView _view;
        [Inject] private SceneManager _sceneManager;
        
        private CompositeDisposable _disposables;
        
        public void Show() =>
            _view.gameObject.SetActive(true);
        public void Hide() =>
            _view.gameObject.SetActive(false);
        
        public void Initialize()
        {
            _disposables = new();
            foreach (var level in _levels)
            {
                _view.CreateLevelButton(level.PlanetIcon, level.PlanetName)
                    .Subscribe(_ =>
                    {
                        _sceneManager.SwitchScene(EScene.Game, di =>
                        {
                            di.Bind<PlanetConfig>()
                                .FromInstance(level.Planet)
                                .AsSingle();
                            di.Bind<ColonizersConfig>()
                                .FromInstance(level.Colonizers)
                                .AsSingle();
                            di.Bind<BuildingsToolbarConfig>()
                                .FromInstance(level.Toolbar)
                                .AsSingle();
                        }).Forget();
                    }).AddTo(_disposables);
            }

            _view.OnExitClick
                .Subscribe(_ => Hide())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}