using Cysharp.Threading.Tasks;
using Jamcelin.Runtime.SceneManagement;
using Runtime.Gameplay.Buildings;
using Runtime.Gameplay.Colonizers;
using Runtime.Gameplay.Planets;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[System.Serializable]
public class LevelCfg
{
    public ColonizersConfig Colonizers;
    public PlanetConfig Planet;
    public BuildingsToolbarConfig Toolbar;
}
public class ChooseLevelView : MonoBehaviour
{
    [SerializeField] private Button _levelOne;
    [SerializeField] private Button _levelTwo;
    [SerializeField] private Button _levelThree;
    [SerializeField] private Button _levelFour;

    [SerializeField] private LevelCfg _levelOneCfg;
    [SerializeField] private LevelCfg _levelTwoCfg;
    [SerializeField] private LevelCfg _levelThreeCfg;
    [SerializeField] private LevelCfg _levelFourCfg;

    [Inject] private SceneManager _manager;
    
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        _levelOne.onClick.AddListener(PressButtonOne);
        _levelTwo.onClick.AddListener(PressButtonTwo);
        _levelThree.onClick.AddListener(PressButtonThree);
        _levelFour.onClick.AddListener(PressButtonFour);
    }

    private void OnDisable()
    {
        _levelOne.onClick.RemoveListener(PressButtonOne);
        _levelTwo.onClick.RemoveListener(PressButtonTwo);
        _levelThree.onClick.RemoveListener(PressButtonThree);
        _levelFour.onClick.RemoveListener(PressButtonFour);
    }

    public void PressButtonOne()
    {
        _manager.SwitchScene(EScene.Game, di =>
        {
            di.Bind<PlanetConfig>()
                .FromInstance(_levelOneCfg.Planet)
                .AsSingle();
            di.Bind<ColonizersConfig>()
                .FromInstance(_levelOneCfg.Colonizers)
                .AsSingle();
            di.Bind<BuildingsToolbarConfig>()
                .FromInstance(_levelOneCfg.Toolbar)
                .AsSingle();
        }).Forget();
    }
    public void PressButtonTwo()
    {
        _manager.SwitchScene(EScene.Game, di =>
        {
            di.Bind<PlanetConfig>()
                .FromInstance(_levelTwoCfg.Planet)
                .AsSingle();
            di.Bind<ColonizersConfig>()
                .FromInstance(_levelTwoCfg.Colonizers)
                .AsSingle();
            di.Bind<BuildingsToolbarConfig>()
                .FromInstance(_levelTwoCfg.Toolbar)
                .AsSingle();
        }).Forget();
    }
    public void PressButtonThree()
    {
        _manager.SwitchScene(EScene.Game, di =>
        {
            di.Bind<PlanetConfig>()
                .FromInstance(_levelThreeCfg.Planet)
                .AsSingle();
            di.Bind<ColonizersConfig>()
                .FromInstance(_levelThreeCfg.Colonizers)
                .AsSingle();
            di.Bind<BuildingsToolbarConfig>()
                .FromInstance(_levelThreeCfg.Toolbar)
                .AsSingle();
        }).Forget();
    }
    public void PressButtonFour()
    {
        _manager.SwitchScene(EScene.Game, di =>
        {
            di.Bind<PlanetConfig>()
                .FromInstance(_levelFourCfg.Planet)
                .AsSingle();
            di.Bind<ColonizersConfig>()
                .FromInstance(_levelFourCfg.Colonizers)
                .AsSingle();
            di.Bind<BuildingsToolbarConfig>()
                .FromInstance(_levelFourCfg.Toolbar)
                .AsSingle();
        }).Forget();
    }
}
