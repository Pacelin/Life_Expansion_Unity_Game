using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class StorageBuildingModel : BuildingModel<StorageBuildingConfig>
    {
        protected override void OnEnable()
        {
            _colonizers.Minerals.ApplyDeltaToMax(_config.IncreaseStorage);
        }

        protected override void OnDisable()
        {
            _colonizers.Minerals.ApplyDeltaToMax(-_config.IncreaseStorage);
        }

        protected override void Dispose() { }
    }
}