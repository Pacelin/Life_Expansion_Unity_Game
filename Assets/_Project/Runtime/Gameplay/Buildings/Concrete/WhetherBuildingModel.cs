using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class WhetherBuildingModel : BuildingModel<WhetherBuildingConfig>
    {
        protected override void OnEnable()
        {
            if (_config.TemperatureDelta != 0)
                _planet.Temperature.ApplyDelta(_config.TemperatureDelta);
            if (_config.OxygenDelta != 0)
                _planet.Oxygen.ApplyDelta(_config.OxygenDelta);
            if (_config.WaterDelta != 0)
                _planet.Water.ApplyDelta(_config.WaterDelta);
        }

        protected override void OnDisable()
        {
            if (_config.TemperatureDelta != 0)
                _planet.Temperature.ApplyDelta(-_config.TemperatureDelta);
            if (_config.OxygenDelta != 0)
                _planet.Oxygen.ApplyDelta(-_config.OxygenDelta);
            if (_config.WaterDelta != 0)
                _planet.Water.ApplyDelta(-_config.WaterDelta);
        }

        protected override void Dispose()
        {
        }
    }
}