using System;
using R3;
using UnityEngine;
using Zenject;

namespace Runtime.MainMenu
{
    public class MainMenuPresenter : IInitializable, IDisposable
    {
        [Inject] private SettingsPresenter _settings;
        [Inject] private LevelSelectionPresenter _levels;
        [Inject] private AboutPresenter _about;
        [Inject] private MainMenuView _view;

        private CompositeDisposable _disposables;
        
        public void Initialize()
        {
            _disposables = new();
            _view.OnExitClick
                .Subscribe(_ => Application.Quit())
                .AddTo(_disposables);
            _view.OnPlayClick
                .Subscribe(_ =>
                {
                    _levels.Show();
                    _about.SetViewActive(false);
                    _settings.SetViewActive(false);
                })
                .AddTo(_disposables);
            _view.OnSettingsClick
                .Subscribe(_ =>
                {
                    _about.SetViewActive(false);
                    _settings.Switch();
                })
                .AddTo(_disposables);
            _view.OnAboutClick
                 .Subscribe(_ =>
                 {
                     _settings.SetViewActive(false);
                     _about.Switch();
                 })
                 .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}