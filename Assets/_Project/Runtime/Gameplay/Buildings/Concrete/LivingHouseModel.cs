using Runtime.Gameplay.Buildings.General;

namespace Runtime.Gameplay.Buildings.Concrete
{
    public class LivingHouseModel : BuildingModel<LivingHouseConfig>
    {
        protected override void OnEnable()
        {
            _colonizers.Population.ApplyDeltaToMax(_config.ColonizersMax);
        }

        protected override void OnDisable()
        {
            _colonizers.Population.ApplyDeltaToMax(-_config.ColonizersMax);
        }

        protected override void Dispose()
        {
        }
    }
}