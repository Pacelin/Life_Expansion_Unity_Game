using R3;

namespace Runtime.Gameplay.Colonizers
{
    public class ColonizersEnergyModel
    {
        public ReadOnlyReactiveProperty<int> Energy => _energy;

        private readonly ReactiveProperty<int> _energy;

        public ColonizersEnergyModel(int initialEnergy) =>
            _energy = new(initialEnergy);

        public void ApplyDelta(int delta) => _energy.Value += delta;
    }
}