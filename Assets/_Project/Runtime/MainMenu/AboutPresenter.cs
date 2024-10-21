using Zenject;

public class AboutPresenter
{
    [Inject]
    private AboutView _view;
    
    public void Switch() =>
        _view.gameObject.SetActive(!_view.gameObject.activeSelf);

    public void SetViewActive(bool value) => _view.gameObject.SetActive(value);
}
