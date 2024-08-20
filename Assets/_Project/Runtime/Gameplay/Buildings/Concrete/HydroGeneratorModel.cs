using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class HydroGeneratorModel : BuildingModel<HydroGeneratorConfig>
    {
        protected override void OnEnable()
        {
            _colonizers.Energy.ApplyDelta(_config.EnergyDelta);
            _planet.Water.ApplyDelta(_config.WaterDelta);
        }

        protected override void OnDisable()
        {
            _colonizers.Energy.ApplyDelta(-_config.EnergyDelta);
            _planet.Water.ApplyDelta(-_config.WaterDelta);
        }

        protected override void Dispose()
        {
        }
    }
}