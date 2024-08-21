using System;
using ModestTree;
using R3;
using Runtime.Cinematic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;

namespace Runtime.MainMenu
{
    public class SettingsPresenter : IInitializable, IDisposable
    {
        [Inject] private SettingsView _view;
        
        private CompositeDisposable _disposables;

        public void Switch() =>
            _view.gameObject.SetActive(!_view.gameObject.activeSelf);
        
        public void Initialize()
        {
            _disposables = new();
            _view.Set(Audio.MasterVolume, Audio.MusicVolume, Audio.SoundVolume);
            _view.MasterVolume.Subscribe(f => Audio.MasterVolume = f)
                .AddTo(_disposables);
            _view.MusicVolume.Subscribe(f => Audio.MusicVolume = f)
                .AddTo(_disposables);
            _view.SoundVolume.Subscribe(f => Audio.SoundVolume = f)
                .AddTo(_disposables);

            _view.OnNextLanguage.Subscribe(_ => ScrollLanguage(1));
            _view.OnPreviousLanguage.Subscribe(_ => ScrollLanguage(-1));
            _view.SetLanguage(LocalizationSettings.SelectedLocale.LocaleName);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
        private void ScrollLanguage(int scroll)
        {
            var locales = LocalizationSettings.AvailableLocales.Locales;
            var curLocaleIndex = locales.IndexOf(LocalizationSettings.SelectedLocale);
            curLocaleIndex = (curLocaleIndex + scroll + locales.Count) % locales.Count; 
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[curLocaleIndex];
            _view.SetLanguage(LocalizationSettings.SelectedLocale.LocaleName);
        }
    }
}