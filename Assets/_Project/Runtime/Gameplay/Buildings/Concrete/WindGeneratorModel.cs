using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class WindGeneratorModel : BuildingModel<WindGeneratorConfig>
    {
        protected override void OnEnable()
        {
            _colonizers.Energy.ApplyDelta(_config.EnergyDelta);
            _planet.Oxygen.ApplyDelta(_config.OxygenDelta);
        }

        protected override void OnDisable()
        {
            _colonizers.Energy.ApplyDelta(-_config.EnergyDelta);
            _planet.Oxygen.ApplyDelta(-_config.OxygenDelta);
        }

        protected override void Dispose()
        {
        }
    }
}