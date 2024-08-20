using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class GeothermalGeneratorModel : BuildingModel<GeothermalGeneratorConfig>
    {
        protected override void OnEnable()
        {
            _colonizers.Energy.ApplyDelta(_config.EnergyDelta);
            _planet.Temperature.ApplyDelta(_config.TemperatureDelta);
        }

        protected override void OnDisable()
        {
            _colonizers.Energy.ApplyDelta(-_config.EnergyDelta);
            _planet.Temperature.ApplyDelta(-_config.TemperatureDelta);
        }

        protected override void Dispose()
        {
        }
    }
}