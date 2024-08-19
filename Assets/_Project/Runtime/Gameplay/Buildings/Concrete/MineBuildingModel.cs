using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class MineBuildingModel : BuildingModel<MineBuildingConfig>
    {
        protected override void OnEnable()
        {
            _planet.Oxygen.ApplyDelta(_config.OxygenDelta);
        }

        protected override void OnDisable()
        {
            _planet.Oxygen.ApplyDelta(-_config.OxygenDelta);
        }

        protected override void Dispose()
        {
        }
    }
}