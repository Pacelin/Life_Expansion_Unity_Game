using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class SolarPanelModel : BuildingModel<SolarPanelConfig>
    {
        protected override void OnEnable()
        {
            _colonizers.Energy.ApplyDelta(_config.EnergyDelta);
        }

        protected override void OnDisable()
        {
            _colonizers.Energy.ApplyDelta(-_config.EnergyDelta);
        }

        protected override void Dispose()
        {
        }
    }
}