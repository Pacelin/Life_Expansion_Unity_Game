using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _about;
    [SerializeField] private GameObject _chooseLevel;
    private GameObject _currentOpen;

    public void OpenSettings()
    {
        _settings.SetActive(true);
        _currentOpen = _settings;
    }

    public void ChooseLevel()
    {
        _chooseLevel.SetActive(true);
        _currentOpen = _chooseLevel;
    }

    public void CloseCurrent()
    {
        _currentOpen.SetActive(false);
    }
}
