using Runtime.Gameplay.Buildings.Builder;
using Runtime.Gameplay.Colonizers;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.General
{
    public class BuildingsValidator : ITickable
    {
        private readonly BuildingFactory _factory;
        private readonly ColonizersModel _colonizers;
        
        public BuildingsValidator(BuildingFactory factory, ColonizersModel colonizers)
        {
            _factory = factory;
            _colonizers = colonizers;
        }

        public void Tick()
        {
            var excessEnergy = _colonizers.Energy.EnergyUsage.CurrentValue - _colonizers.Energy.Energy.CurrentValue;
            var excessColonizers = _colonizers.Population.BusyPopulation.CurrentValue - _colonizers.Population.CurrentPopulation.CurrentValue;
            for (int i = _factory.Buildings.Count - 1; i >= 0; i--)
            {
                var building = _factory.Buildings[i];
                if (building.Enabled)
                {
                    if (excessEnergy > 0)
                    {
                        excessEnergy -= building.EnergyCost;
                        if (excessColonizers > 0)
                            building.SetState(EBuildingState.NoEnergy, EBuildingState.NoColonists);
                        else
                            building.SetState(EBuildingState.NoEnergy);
                        excessColonizers -= building.ColonizersCost;
                    }
                    else if (excessColonizers > 0)
                    {
                        excessEnergy -= building.EnergyCost;
                        excessColonizers -= building.ColonizersCost;
                        building.SetState(EBuildingState.NoColonists);
                    }
                }
                else
                {
                    if (Mathf.Abs(excessEnergy) >= building.EnergyCost &&
                        Mathf.Abs(excessColonizers) >= building.ColonizersCost)
                    {
                        building.CancelState(EBuildingState.NoColonists);
                        building.CancelState(EBuildingState.NoEnergy);
                        excessEnergy += building.EnergyCost;
                        excessColonizers += building.ColonizersCost;
                    }
                }
            }
        }
    }
}