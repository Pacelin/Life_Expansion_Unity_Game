using System;
using Cysharp.Threading.Tasks;
using Jamcelin.Runtime.SceneManagement;
using Runtime.Gameplay.Buildings.General;
using Runtime.Gameplay.Buildings.UI;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class CosmoportModel : BuildingModel<CosmoportConfig>
    {
        [Inject] private SceneManager _manager;
        protected override async void OnEnable()
        {
            CongratulationView.Instance.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            _manager.SwitchScene(EScene.MainMenu, _ => {}).Forget();
        }

        protected override void OnDisable()
        {
        }

        protected override void Dispose()
        {
        }
    }
}