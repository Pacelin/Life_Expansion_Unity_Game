using R3;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Runtime.MainMenu
{
    public class LevelSelectionView : MonoBehaviour
    {
        public Observable<Unit> OnExitClick => _exitButton.OnClickAsObservable();
        
        [SerializeField] private Button _exitButton;
        [SerializeField] private RectTransform _buttonsContainer;
        [SerializeField] private LevelButtonView _buttonPrefab;

        public Observable<Unit> CreateLevelButton(Sprite icon, LocalizedString caption)
        {
            var view = Instantiate(_buttonPrefab, _buttonsContainer);
            view.Set(icon, caption);
            return view.OnClick;
        }
    }
}