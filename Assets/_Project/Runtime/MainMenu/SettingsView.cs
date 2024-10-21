using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.MainMenu
{
    public class SettingsView : MonoBehaviour
    {
        public Observable<float> MasterVolume => _masterVolume.onValueChanged.AsObservable();
        public Observable<float> MusicVolume => _musicVolume.onValueChanged.AsObservable();
        public Observable<float> SoundVolume => _soundVolume.onValueChanged.AsObservable();

        public Observable<Unit> OnNextLanguage => _nextLanguageButton.OnClickAsObservable();
        public Observable<Unit> OnPreviousLanguage => _previousLanguageButton.OnClickAsObservable();

        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _soundVolume;
        [SerializeField] private TextMeshProUGUI _languageText;
        [SerializeField] private Button _nextLanguageButton;
        [SerializeField] private Button _previousLanguageButton;
        
        public void Set(float master, float music, float sound)
        {
            _masterVolume.value = master;
            _musicVolume.value = music;
            _soundVolume.value = sound;
        }

        public void SetLanguage(string language)
        {
            
        }
    }
}