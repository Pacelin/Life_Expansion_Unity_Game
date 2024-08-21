using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        public Observable<Unit> OnPlayClick => _playButton.OnClickAsObservable();
        public Observable<Unit> OnSettingsClick => _settingsButton.OnClickAsObservable();
        public Observable<Unit> OnAboutClick => _aboutButton.OnClickAsObservable();
        public Observable<Unit> OnExitClick => _exitButton.OnClickAsObservable();

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _aboutButton;
        [SerializeField] private Button _exitButton;
    }
}