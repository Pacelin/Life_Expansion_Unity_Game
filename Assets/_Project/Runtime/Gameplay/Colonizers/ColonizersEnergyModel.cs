using R3;

namespace Runtime.Gameplay.Colonizers
{
    public class ColonizersEnergyModel
    {
        public ReadOnlyReactiveProperty<int> Energy => _energy;
        public ReadOnlyReactiveProperty<int> EnergyUsage => _energyUsage;

        private readonly ReactiveProperty<int> _energyUsage;
        private readonly ReactiveProperty<int> _energy;

        public ColonizersEnergyModel(int initialEnergy)
        {
            _energy = new(initialEnergy);
            _energyUsage = new(0);
        }

        public void ApplyDelta(int delta) => _energy.Value += delta;
        public void ApplyDeltaToUsage(int delta) => _energyUsage.Value += delta;
    }
}