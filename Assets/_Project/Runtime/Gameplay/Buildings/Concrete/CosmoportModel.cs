using Cysharp.Threading.Tasks;
using Jamcelin.Runtime.SceneManagement;
using Runtime.Gameplay.Buildings.General;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class CosmoportModel : BuildingModel<CosmoportConfig>
    {
        [Inject] private SceneManager _manager;
        protected override void OnEnable()
        {
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