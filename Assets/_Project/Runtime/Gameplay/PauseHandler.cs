using Cysharp.Threading.Tasks;
using Jamcelin.Runtime.SceneManagement;
using Runtime.Cinematic;
using Runtime.Gameplay.Buildings;
using Runtime.Gameplay.Colonizers;
using Runtime.Gameplay.Planets;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class PauseHandler : MonoBehaviour
{
    private bool _gamePaused;

    [SerializeField]
    private UnityEvent _onPause;
    [SerializeField]
    private UnityEvent _onResume;

    [Inject]
    private SceneManager _sceneManager;

    [Inject]
    private PlanetConfig _planetConfig;
    [Inject]
    private ColonizersConfig _colonizersConfig;
    [Inject]
    private BuildingsToolbarConfig _buildingsToolbarConfig;
    [Inject]
    private CameraController _cameraController;
    
    public void Pause()
    {
        _gamePaused = true;
        Time.timeScale = 0f;
        _cameraController.Enabled = false;
        
        _onPause?.Invoke();
    }

    public void Resume()
    {
        _gamePaused = false;
        Time.timeScale = 1f;
        _cameraController.Enabled = true;
        
        _onResume?.Invoke();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        _sceneManager.SwitchScene(EScene.Game, di =>
        {
            di.Bind<PlanetConfig>()
              .FromInstance(_planetConfig)
              .AsSingle();
            di.Bind<ColonizersConfig>()
              .FromInstance(_colonizersConfig)
              .AsSingle();
            di.Bind<BuildingsToolbarConfig>()
              .FromInstance(_buildingsToolbarConfig)
              .AsSingle();
        }).Forget();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        _sceneManager.SwitchScene(EScene.MainMenu, _ => { }).Forget();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (_gamePaused)
            Resume();
        else
            Pause();
    }
}
